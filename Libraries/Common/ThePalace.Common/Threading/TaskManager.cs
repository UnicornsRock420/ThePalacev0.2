using System.Collections.Concurrent;
using ThePalace.Common.Factories;
using ThePalace.Common.Interfaces.Threading;
using static ThePalace.Common.Threading.Job;

namespace ThePalace.Common.Threading;

public partial class TaskManager : SingletonDisposable<TaskManager>
{
    static TaskManager() => _globalToken = CancellationTokenFactory.NewToken();

    public TaskManager() => _jobs = new();

    ~TaskManager() => Dispose();

    public override void Dispose()
    {
        if (!_globalToken.IsCancellationRequested)
        {
            try { _globalToken.CancelAsync(); } catch { }

            Thread.Sleep(450);

            _jobs.Values
                .Union(_jobs.Values
                    .SelectMany(j => j._subJobs ?? []))
                .ToList()
                .ForEach(j =>
                {
                    try { j.Cancel(); } catch { }
                    try { j.Dispose(); } catch { }

                    j.JobState = null;
                });
            _jobs.Clear();

            Thread.Sleep(450);

            _globalToken.Dispose();
        }

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    private static readonly CancellationTokenSource _globalToken;

    private readonly Dictionary<Guid, Job> _jobs;
    public IReadOnlyDictionary<Guid, Job> Jobs => _jobs.AsReadOnly();

    public static CancellationToken GlobalToken => _globalToken.Token;

    public Job CreateTask(Action<ConcurrentQueue<Cmd>> cmd, IJobState? jobState, RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null)
    {
        if (_globalToken.IsCancellationRequested) return null;

        sleepInterval ??= TimeSpan.FromMilliseconds(750);

        var job = new Job(cmd, jobState, opts, sleepInterval);

        _jobs.Add(job.Id, job);

        if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
        {
            job.Task.Start();
        }

        return job;
    }

    public async Task Fork(Job parent, int threadCount = 1, RunOptions opts = RunOptions.RunNow)
    {
        if (_globalToken.IsCancellationRequested ||
            threadCount < 1 ||
            !_jobs.ContainsKey(parent.Id)) return;

        for (var j = threadCount; j > 0; j--)
        {
            var job = new Job(parent);
            parent._subJobs.Add(job);

            if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
            {
                job.Run();
            }
        }
    }

    public bool Cancel(Guid jobId, CancelOptions opts = CancelOptions.Cascade)
    {
        if (!_jobs.ContainsKey(jobId)) return false;

        _jobs[jobId].Cancel(opts);

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
        var jobs = _jobs.Values
            .SelectMany(j => j._subJobs)
            .ToList();

        foreach (var job in jobs)
        {
            if (_expiredStates.Contains(job.Task.Status))
            {
                try { job.Cancel(); } catch { }
                try { job.Dispose(); } catch { }

                if (_jobs.ContainsKey(job.Id))
                {
                    _jobs.Remove(job.Id);
                }
                else if (
                    job.ParentId.HasValue &&
                    _jobs.ContainsKey(job.ParentId.Value))
                {
                    _jobs[job.ParentId.Value]._subJobs.Remove(job);
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

            var jobs = _jobs.Values
                .Union(_jobs.Values
                    .SelectMany(j => j._subJobs))
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