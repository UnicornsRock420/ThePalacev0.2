using System.Collections.Concurrent;
using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Common.Threading;

[Flags]
public enum RunOptions : int
{
    None = 0,
    BreakOnError = 0x01,
    RunNow = 0x02,
    RunOnce = 0x04,
    UseSleepInterval = 0x08,
    UseTimer = 0x10,
    UseResetEvent = 0x20,
}

[Flags]
public enum CancelOptions : int
{
    OnlyMain = 0x01,
    OnlyChildren = 0x02,
    Cascade = OnlyMain | OnlyChildren,
}

public partial class RunLog
{
    public DateTime Start { get; set; }
    public DateTime? Finish { get; set; }
    public DateTime? Error { get; set; }
    public Exception? Exception { get; set; }
    public TimeSpan Duration => (Finish ?? Error ?? DateTime.UtcNow).Subtract(Start);
}

public partial class Job<TCmd> : IJob<TCmd>, IDisposable
    where TCmd : ICmd
{
    private const int CONST_defaultSleepInterval = 1500;

    public Job()
    {
        TokenSource = CancellationTokenFactory.NewToken();
        SubJobs = new();
        RunLogs = new();
        Errors = new();

        IsRunning = false;

        Completions = 0;
        Failures = 0;

        SleepInterval = TimeSpan.FromMilliseconds(CONST_defaultSleepInterval);
        Queue = new ConcurrentQueue<TCmd>();
        JobState = null;
    }
    public Job(Action<ConcurrentQueue<TCmd>>? cmd = null, IJobState? jobState = null, RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null, Interfaces.Threading.ITimer? timer = null) : this()
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));

        sleepInterval ??= TimeSpan.FromMilliseconds(CONST_defaultSleepInterval);

        JobState = jobState;
        Options = opts;

        if (sleepInterval != null)
        {
            SleepInterval = sleepInterval.Value;
        }

        if ((opts & RunOptions.UseTimer) == RunOptions.UseTimer &&
            timer != null)
        {
            Timer = timer;
            Timer.Interval = (int)sleepInterval.Value.TotalMilliseconds;
            Timer.Tick += new EventHandler((s, a) =>
            {
                this.TimerRun();
            });
        }

        if ((opts & RunOptions.UseResetEvent) == RunOptions.UseResetEvent)
        {
            ResetEvent = new(false);
        }

        Build(Cmd = cmd);
    }

    internal Job(IJob<TCmd> parent) : this(parent.Cmd, parent.JobState, parent.Options, parent.SleepInterval, parent.Timer)
    {
        ParentId = parent.Id;
        Cmd = parent.Cmd;
        JobState = parent.JobState;
        Options = parent.Options;
    }

    ~Job() => Dispose();

    public void Dispose()
    {
        try { ResetEvent?.Dispose(); } catch { } finally { ResetEvent = null; }
        try { TokenSource?.Dispose(); } catch { } finally { TokenSource = null; }
        try { Task?.Dispose(); } catch { } finally { Task = null; }

        SubJobs?.ToList()?.ForEach(j => { try { j?.Dispose(); } catch { } });
        SubJobs?.Clear();

        RunLogs?.ToList()?.ForEach(l => { try { l.Exception = null; } catch { } });
        RunLogs?.Clear();
        RunLogs = null;

        Errors?.Clear();
        Errors = null;

        Queue?.ToList()?.ForEach(l => { if (l is IDisposable i) try { i?.Dispose(); } catch { } });
        Queue?.Clear();
        Queue = null;

        Timer = null;
        JobState = null;

        GC.SuppressFinalize(this);
    }

    public virtual Action<ConcurrentQueue<TCmd>> Cmd { get; protected set; }
    public Task Task { get; protected set; }

    public int LogLimit { get; set; } = 20;
    public List<RunLog> RunLogs { get; protected set; }

    public int ErrorLimit { get; set; } = 50;
    public List<Exception> Errors { get; protected set; }

    public CancellationTokenSource TokenSource { get; protected set; }
    public CancellationToken Token => TokenSource.Token;

    DisposableList<IJob> IJob.SubJobs => new(SubJobs.Cast<IJob>());
    public DisposableList<IJob<TCmd>> SubJobs { get; protected set; }

    public RunOptions Options { get; set; }
    public Guid Id { get; } = Guid.NewGuid();
    public Guid? ParentId { get; protected set; }
    public bool IsRunning { get; protected set; }
    public int Completions { get; protected set; }
    public int Failures { get; protected set; }
    public ManualResetEvent ResetEvent { get; protected set; }
    public TimeSpan SleepInterval { get; set; }
    public Interfaces.Threading.ITimer Timer { get; set; }
    public ConcurrentQueue<TCmd> Queue { get; protected set; }
    public virtual IJobState? JobState { get; set; }

    public void Cancel(CancelOptions opts = CancelOptions.Cascade)
    {
        var doCascade = (opts & CancelOptions.Cascade) == CancelOptions.Cascade;
        var jobs = new List<IJob<TCmd>>();

        if (doCascade || (opts & CancelOptions.OnlyMain) == CancelOptions.OnlyMain)
        {
            jobs.Add(this);
        }

        if (doCascade || (opts & CancelOptions.OnlyChildren) == CancelOptions.OnlyChildren)
        {
            jobs.AddRange(this?.SubJobs
                ?.SelectMany(j => j?.SubJobs ?? []) ?? []);
        }

        jobs.ForEach(t => t?.Dispose());
    }


    public void Build(Action<ConcurrentQueue<TCmd>>? cmd = null, CancellationToken? token = null)
    {
        if (Task != null)
        {
            try { Cancel(); } catch { }
            try { Task?.Dispose(); } catch { }
        }

        Task = new Task(() =>
        {
            (cmd ?? Cmd)(Queue);
        }, token ?? TokenSource.Token);
    }

    protected int TimerRun()
    {
        var doUseTimer = (Options & RunOptions.UseTimer) == RunOptions.UseTimer;
        if (!doUseTimer) return -1;

        IsRunning = true;

        if (Task.Status != TaskStatus.Running)
            try
            {
                Cmd(Queue);

                IsRunning = false;
                Completions++;
            }
            catch (Exception ex)
            {
                IsRunning = false;
                Failures++;

                return -1;
            }

        return Failures > 0 ? -1 : 0;
    }

    public async Task<int> Run()
    {
        var doUseTimer = (Options & RunOptions.UseTimer) == RunOptions.UseTimer;
        if (doUseTimer)
        {
            if (Timer != null &&
                !Timer.Enabled)
            {
                Timer.Start();
            }

            return 0;
        }

        var doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
        var doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
        var doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
        var doUseManualResetEvent = (Options & RunOptions.UseResetEvent) == RunOptions.UseResetEvent;

        if (doUseManualResetEvent)
        {
            ResetEvent.Reset();
        }

        while (!TokenSource.IsCancellationRequested)
        {
            var runLog = new RunLog
            {
                Start = DateTime.UtcNow,
            };
            IsRunning = true;

            try
            {
                Task.Start();

                runLog.Finish = DateTime.UtcNow;

                IsRunning = false;
                Completions++;
            }
            catch (Exception ex)
            {
                runLog.Error = DateTime.UtcNow;

                IsRunning = false;
                Failures++;

                if (Errors.Count >= LogLimit)
                    Errors.RemoveAt(0);

                Errors.Add(runLog.Exception = ex);
            }

            TokenSource.Token.ThrowIfCancellationRequested();

            if (RunLogs.Count >= ErrorLimit)
                RunLogs.RemoveAt(0);

            RunLogs.Add(runLog);

            if (doRunRunOnce && runLog.Finish.HasValue) return 0;

            if ((doRunRunOnce || doBreakOnError) && runLog.Error.HasValue) return -1;

            TokenSource.Token.ThrowIfCancellationRequested();

            if (doUseManualResetEvent)
            {
                ResetEvent.WaitOne();
            }
            else if (doUseSleepInterval)
            {
                var timer = DateTime.UtcNow;

                await Task.Delay(SleepInterval);

                if (DateTime.UtcNow.Subtract(timer).TotalMilliseconds < SleepInterval.TotalMilliseconds * 0.70)
                {
                    Thread.Sleep(SleepInterval);
                }
            }

            doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
            doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
            doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
            doUseManualResetEvent = (Options & RunOptions.UseResetEvent) == RunOptions.UseResetEvent;
        }

        return Failures > 0 ? -1 : 0;
    }

    public void Enqueue(TCmd cmd)
    {
        Queue.Enqueue(cmd);
    }
}