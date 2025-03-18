using System.Collections;
using System.Collections.Concurrent;
using Lib.Common.Interfaces.Threading;
using ITimer = Lib.Common.Interfaces.Threading.ITimer;

namespace Lib.Common.Threading;

[Flags]
public enum RunOptions
{
    None = 0,
    BreakOnError = 0x01,
    DoRunLog = 0x02,
    RunNow = 0x04,
    RunOnce = 0x08,
    UseResetEvent = 0x10,
    UseSleepInterval = 0x20,
    UseTimer = 0x40
}

[Flags]
public enum CancelOptions
{
    OnlyMain = 0x01,
    OnlyChildren = 0x02,
    Cascade = OnlyMain | OnlyChildren
}

public class RunLog
{
    public RunLog()
    {
    }

    public RunLog(bool begin = true)
    {
        if (begin)
            Start();
    }

    public DateTime Began { get; set; }
    public DateTime? Finish { get; set; }
    public DateTime? Error { get; set; }
    public Exception? Exception { get; set; }
    public TimeSpan Duration => (Finish ?? Error ?? DateTime.UtcNow).Subtract(Began);

    public void Start()
    {
        Began = DateTime.UtcNow;
    }

    public void Stop(Exception? ex = null)
    {
        if (Finish != null ||
            Error != null) return;

        if (ex == null)
        {
            Finish = DateTime.UtcNow;
        }
        else
        {
            Error = DateTime.UtcNow;
            Exception = ex;
        }
    }
}

public class Job<TCmd> : Disposable, IJob<TCmd>, IDisposable
    where TCmd : class, ICmd
{
    private const int CONST_defaultSleepInterval = 1500;

    public Job()
    {
        TokenSource = CancellationTokenFactory.NewToken();
        SubJobs = new();
        Queue = new();
        Errors = [];

        JobState = null;

        Completions = 0;
        Failures = 0;
        IsRunning = false;

        _managedResources.AddRange([TokenSource]);
    }

    public Job(
        Action<ConcurrentQueue<TCmd>>? cmd = null,
        IJobState? jobState = null,
        RunOptions opts = RunOptions.UseSleepInterval,
        TimeSpan? sleepInterval = null,
        ITimer? timer = null,
        CancellationToken? token = null)
    {
        ArgumentNullException.ThrowIfNull(cmd, nameof(cmd));

        TokenSource = CancellationTokenFactory.NewToken();
        SubJobs = new();
        Queue = new();
        Errors = [];

        Completions = 0;
        Failures = 0;
        IsRunning = false;

        _managedResources.AddRange([TokenSource]);
        JobState = jobState;
        Options = opts;

        if ((opts & RunOptions.DoRunLog) == RunOptions.DoRunLog)
        {
            RunLogs = [];
        }

        sleepInterval ??= TimeSpan.FromMilliseconds(CONST_defaultSleepInterval);
        if ((opts & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval &&
            sleepInterval != null)
            SleepInterval = sleepInterval.Value;

        if ((opts & RunOptions.UseTimer) == RunOptions.UseTimer &&
            timer != null)
        {
            Timer = timer;
            Timer.Interval = (int)sleepInterval.Value.TotalMilliseconds;
            Timer.Tick += (s, a) => { TimerRun(); };
        }

        if ((opts & RunOptions.UseResetEvent) == RunOptions.UseResetEvent)
            ResetEvent = new(false);

        Build(Cmd = cmd, token);
    }

    internal Job(IJob<TCmd> parent) : this(parent.Cmd, parent.JobState, parent.Options, parent.SleepInterval, parent.Timer)
    {
        ParentId = parent.Id;
        Cmd = parent.Cmd;
        JobState = parent.JobState;
        Options = parent.Options;
    }

    ~Job()
    {
        Dispose();
    }

    public override void Dispose()
    {
        try
        {
            ResetEvent?.Dispose();
        }
        catch
        {
        }

        ResetEvent = null;

        try
        {
            Task?.Dispose();
        }
        catch
        {
        }

        Task = null;

        SubJobs?.ToList()?.ForEach(j =>
        {
            try
            {
                j?.Dispose();
            }
            catch
            {
            }
        });
        SubJobs?.Clear();
        SubJobs = null;

        RunLogs?.ToList()?.ForEach(l =>
        {
            try
            {
                l.Exception = null;
            }
            catch
            {
            }
        });
        RunLogs?.Clear();
        RunLogs = null;

        Errors?.Clear();
        Errors = null;

        Queue?.ToList()?.ForEach(l =>
        {
            if (l is not IDisposable i) return;

            try
            {
                i?.Dispose();
            }
            catch
            {
            }
        });
        Queue?.Clear();
        Queue = null;

        Timer = null;
        JobState = null;
        ResetEvent = null;
        TokenSource = null;
        Task = null;
        Cmd = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    #region Task/Thread Properties

    public Guid Id { get; } = Guid.NewGuid();
    public Guid? ParentId { get; protected set; }

    public RunOptions Options { get; set; }

    public CancellationTokenSource TokenSource { get; protected set; }
    public CancellationToken Token => TokenSource.Token;

    public virtual Action<ConcurrentQueue<TCmd>> Cmd { get; protected set; }
    public Task Task { get; protected set; }

    public DisposableList<IJob<TCmd>> SubJobs { get; protected set; }
    DisposableList<IJob> IJob.SubJobs => new(SubJobs);

    public ManualResetEvent ResetEvent { get; protected set; }
    public TimeSpan SleepInterval { get; set; }
    public ITimer Timer { get; set; }

    public virtual ConcurrentQueue<TCmd> Queue { get; protected set; }
    public virtual IJobState? JobState { get; set; }

    #endregion

    #region Logging

    public bool IsRunning { get; protected set; }
    public int Completions { get; protected set; }
    public int Failures { get; protected set; }

    public int LogLimit { get; set; } = 20;
    public List<RunLog> RunLogs { get; protected set; }

    public int ErrorLimit { get; set; } = 50;
    public List<Exception> Errors { get; protected set; }

    #endregion

    public void Cancel(CancelOptions opts = CancelOptions.Cascade)
    {
        var doCascade = (opts & CancelOptions.Cascade) == CancelOptions.Cascade;
        var jobs = new List<IJob<TCmd>>();

        if (doCascade || (opts & CancelOptions.OnlyMain) == CancelOptions.OnlyMain) jobs.Add(this);

        if (doCascade || (opts & CancelOptions.OnlyChildren) == CancelOptions.OnlyChildren)
            jobs.AddRange(this?.SubJobs
                ?.SelectMany(j => j?.SubJobs ?? []) ?? []);

        jobs.ForEach(j =>
        {
            if (j?.Task?.Status != TaskStatus.Running &&
                j?.Task?.Status != TaskStatus.WaitingForChildrenToComplete) return;
            
            try
            {
                j?.TokenSource?.Cancel();
            }
            catch
            {
            }
        });
    }

    public void Build(Action<ConcurrentQueue<TCmd>>? cmd = null, CancellationToken? token = null)
    {
        if (IsDisposed) return;

        if (Task != null)
        {
            try
            {
                Cancel();
            }
            catch
            {
            }

            try
            {
                Task?.Dispose();
            }
            catch
            {
            }
        }

        Task = new Task(() => { (cmd ?? Cmd)(Queue); }, token ?? TokenSource.Token);
    }

    protected int TimerRun()
    {
        if (IsDisposed) return 0;

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
        if (IsDisposed) return 0;

        if (Task == null) throw new NullReferenceException(nameof(Job<TCmd>) + "." + nameof(Task));

        if ((Options & RunOptions.UseTimer) == RunOptions.UseTimer)
        {
            if (!(Timer?.Enabled ?? true))
                Timer.Start();

            return 0;
        }

        var doRunLog = (Options & RunOptions.DoRunLog) == RunOptions.DoRunLog;
        var doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
        var doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
        var doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
        var doUseManualResetEvent = (Options & RunOptions.UseResetEvent) == RunOptions.UseResetEvent;

        if (doUseManualResetEvent) ResetEvent.Reset();

        var runLog = (RunLog?)null;

        while (!TokenSource.IsCancellationRequested)
        {
            if (doRunLog)
                runLog = new RunLog(true);

            switch (Task.Status)
            {
                case TaskStatus.Canceled:
                case TaskStatus.Faulted:
                    return -1;
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                    break;
                case TaskStatus.RanToCompletion:
                    Build(Cmd, Token);

                    goto default;
                default:
                    try
                    {
                        IsRunning = true;

                        Task.Start();

                        if (doRunLog)
                            runLog.Stop();

                        Completions++;
                    }
                    catch (Exception ex)
                    {
                        if (doRunLog)
                            runLog.Stop(ex);

                        Failures++;

                        if (Errors.Count >= ErrorLimit)
                            Errors.RemoveAt(0);

                        Errors.Add(ex);
                    }
                    finally
                    {
                        if (doRunLog)
                            runLog.Stop();

                        IsRunning = false;
                    }

                    TokenSource.Token.ThrowIfCancellationRequested();

                    if (doRunLog)
                    {
                        if (RunLogs.Count >= LogLimit)
                            RunLogs.RemoveAt(0);

                        RunLogs.Add(runLog);
                    }

                    if (doRunRunOnce && runLog.Finish.HasValue) return 0;

                    if ((doRunRunOnce || doBreakOnError) && runLog.Error.HasValue) return -1;
                    break;
            }

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
                    Thread.Sleep(SleepInterval);
            }

            doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
            doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
            doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
            doUseManualResetEvent = (Options & RunOptions.UseResetEvent) == RunOptions.UseResetEvent;
        }

        return Failures > 0 ? -1 : 0;
    }

    public void Enqueue(TCmd cmd, bool clear = false)
    {
        if (IsDisposed) return;

        if (clear)
        {
            Queue.Clear();
        }

        if ((Options & RunOptions.UseResetEvent) == RunOptions.UseResetEvent)
        {
            ResetEvent.Set();
        }

        Queue.Enqueue(cmd);
    }
}