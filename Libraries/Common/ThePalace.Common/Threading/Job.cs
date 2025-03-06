using System.Collections.Concurrent;
using System.Collections.Generic;
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
    UseManualResetEvent = 0x10,
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
    public Job()
    {
        TokenSource = CancellationTokenFactory.NewToken();
        SubJobs = new();
        RunLogs = new();
        Errors = new();

        IsRunning = false;

        Completions = 0;
        Failures = 0;

        SleepInterval = TimeSpan.FromMilliseconds(1500);
        Queue = new ConcurrentQueue<TCmd>();
        JobState = null;
    }
    public Job(Action<ConcurrentQueue<TCmd>>? cmd = null, IJobState? jobState = null, RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null) : this()
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));

        sleepInterval ??= TimeSpan.FromMilliseconds(750);

        JobState = jobState;
        Options = opts;

        if (sleepInterval != null)
        {
            SleepInterval = sleepInterval.Value;
        }

        if ((opts & RunOptions.UseManualResetEvent) == RunOptions.UseManualResetEvent)
        {
            ResetEvent = new(false);
        }

        Build(Cmd = cmd);
    }
    
    internal Job(IJob<TCmd> parent) : this(parent.Cmd, parent.JobState, parent.Options, parent.SleepInterval)
    {
        ParentId = parent.Id;
        Cmd = parent.Cmd;
        JobState = parent.JobState;
        Options = parent.Options;
    }

    ~Job() => Dispose();

    public void Dispose()
    {
        try { ResetEvent?.Dispose(); } catch { }
        try { TokenSource?.Dispose(); } catch { }
        try { Task?.Dispose(); } catch { }

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
    public new DisposableList<IJob<TCmd>> SubJobs { get; protected set; }

    public RunOptions Options { get; set; }
    public Guid Id { get; } = Guid.NewGuid();
    public Guid? ParentId { get; protected set; }
    public bool IsRunning { get; protected set; }
    public int Completions { get; protected set; }
    public int Failures { get; protected set; }
    public ManualResetEvent ResetEvent { get; protected set; }
    public TimeSpan SleepInterval { get; set; }
    public virtual ConcurrentQueue<TCmd> Queue { get; protected set; }
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
            jobs.AddRange(this.SubJobs
                .SelectMany(j => j.SubJobs));
        }

        jobs.ForEach(t => t.Cancel());
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
    
    public async Task<int> Run()
    {
        var doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
        var doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
        var doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
        var doUseManualResetEvent = (Options & RunOptions.UseManualResetEvent) == RunOptions.UseManualResetEvent;

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
            doUseManualResetEvent = (Options & RunOptions.UseManualResetEvent) == RunOptions.UseManualResetEvent;
        }

        return Failures > 0 ? -1 : 0;
    }

    public void Enqueue(TCmd cmd)
    {
        Queue.Enqueue(cmd);
    }
}