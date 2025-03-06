using System.Collections.Concurrent;
using ThePalace.Common.Factories;
using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Common.Threading;

public partial class TaskManager : SingletonDisposable<TaskManager>
{
    static TaskManager() => _globalToken = CancellationTokenFactory.NewToken();

    public TaskManager() => Jobs = new();

    ~TaskManager() => Dispose();

    public override void Dispose()
    {
        if (!_globalToken.IsCancellationRequested)
        {
            try { _globalToken.CancelAsync(); } catch { }

            Thread.Sleep(450);

            Jobs.Values
                .Union(Jobs.Values
                    .SelectMany(j => j.SubJobs ?? []))
                .ToList()
                .ForEach(j =>
                {
                    try { j.Cancel(); } catch { }
                    try { j.Dispose(); } catch { }

                    j.JobState = null;
                });
            Jobs.Clear();

            Thread.Sleep(450);

            _globalToken.Dispose();
        }

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    private static readonly CancellationTokenSource _globalToken;

    public Dictionary<Guid, IJob> Jobs { get; protected set; }

    public static CancellationToken GlobalToken => _globalToken.Token;

    public IJob CreateTask(Action<ConcurrentQueue<ICmd>> cmd, IJobState? jobState, RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null)
    {
        if (_globalToken.IsCancellationRequested) return null;

        sleepInterval ??= TimeSpan.FromMilliseconds(750);

        var job = new Job<ICmd>(cmd, jobState, opts, sleepInterval);

        Jobs.Add(job.Id, job);

        if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
        {
            job.Task.Start();
        }

        return job;
    }

    public Job<TCmd> CreateTask<TCmd>(Action<ConcurrentQueue<TCmd>> cmd, IJobState? jobState, RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null)
        where TCmd : ICmd
    {
        if (_globalToken.IsCancellationRequested) return null;

        sleepInterval ??= TimeSpan.FromMilliseconds(750);

        var job = new Job<TCmd>(cmd, jobState, opts, sleepInterval);

        Jobs.Add(job.Id, job);

        if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
        {
            job.Task.Start();
        }

        return job;
    }

    public async Task Fork(IJob<ICmd> parent, int threadCount = 1, RunOptions opts = RunOptions.RunNow)
    {
        if (_globalToken.IsCancellationRequested ||
            threadCount < 1 ||
            !Jobs.ContainsKey(parent.Id)) return;

        for (var j = threadCount; j > 0; j--)
        {
            var job = new Job<ICmd>(parent);
            parent.SubJobs.Add(job);

            if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
            {
                job.Run();
            }
        }
    }

    public bool Cancel(Guid jobId, CancelOptions opts = CancelOptions.Cascade)
    {
        if (!Jobs.ContainsKey(jobId)) return false;

        Jobs[jobId].Cancel(opts);

        return true;
    }

    private static readonly TaskStatus[] _expiredStates =
    [
        TaskStatus.Canceled,
        TaskStatus.Faulted,
        TaskStatus.RanToCompletion,
    ];
    private void Cleanup()
    {
        var jobs = Jobs.Values
            .SelectMany(j => j.SubJobs)
            .ToList();

        foreach (var job in jobs)
        {
            if (_expiredStates.Contains(job.Task.Status))
            {
                try { job.Cancel(); } catch { }
                try { job.Dispose(); } catch { }

                if (Jobs.ContainsKey(job.Id))
                {
                    Jobs.Remove(job.Id);
                }
                else if (
                    job.ParentId.HasValue &&
                    Jobs.ContainsKey(job.ParentId.Value))
                {
                    Jobs[job.ParentId.Value].SubJobs.Remove(job);
                }
            }
        }
    }

    public void Run(TimeSpan? sleepInterval = null, CancellationToken? token = null, params IDisposable[] resources)
    {
        if ((resources?.Length ?? 0) > 0)
        {
            _managedResources.AddRange(resources);
        }

        sleepInterval ??= TimeSpan.FromMilliseconds(750);

        while (!GlobalToken.IsCancellationRequested &&
               (!token.HasValue || !token.Value.IsCancellationRequested))
        {
            Cleanup();

            var jobs = Jobs.Values
                .Union(Jobs.Values
                    .SelectMany(j => j.SubJobs))
                .ToList();
            foreach (var job in jobs)
            {
                if (!_expiredStates.Contains(job.Task.Status) &&
                    job.Task.Status != TaskStatus.Running)
                {
                    try { job.Run(); } catch { }
                }
            }

            Task.WaitAny(jobs.Select(j => j.Task).ToArray());

            Cleanup();

            GlobalToken.ThrowIfCancellationRequested();

            if (token.HasValue)
                token.Value.ThrowIfCancellationRequested();

            Thread.Sleep((int)sleepInterval.Value.TotalMilliseconds);
        }
    }

    public void Shutdown() => Dispose();
}