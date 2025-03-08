using System.Collections.Concurrent;
using ThePalace.Common.Factories;
using ThePalace.Common.Interfaces.Threading;
using ITimer = ThePalace.Common.Interfaces.Threading.ITimer;

namespace ThePalace.Common.Threading;

public class TaskManager : SingletonDisposable<TaskManager>
{
    private const int CONST_defaultSleepInterval = 1500;

    private static readonly CancellationTokenSource _globalToken = CancellationTokenFactory.NewToken();

    private static readonly TaskStatus[] _expiredStates =
    [
        TaskStatus.Canceled,
        TaskStatus.Faulted,
        TaskStatus.RanToCompletion
    ];

    public TaskManager()
    {
    }

    ~TaskManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (!_globalToken.IsCancellationRequested)
        {
            try
            {
                _globalToken.CancelAsync();
            }
            catch
            {
            }

            Thread.Sleep(450);

            Jobs.Values
                .Union(Jobs.Values
                    .SelectMany(j => j.SubJobs ?? []))
                .ToList()
                .ForEach(j =>
                {
                    try
                    {
                        j.Cancel();
                    }
                    catch
                    {
                    }

                    try
                    {
                        j.Dispose();
                    }
                    catch
                    {
                    }

                    j.JobState = null;
                });
            Jobs.Clear();

            Thread.Sleep(450);

            _globalToken.Dispose();
        }

        base.Dispose();
    }

    public Dictionary<Guid, IJob> Jobs { get; protected set; } = new();

    public static CancellationToken GlobalToken => _globalToken.Token;

    public IJob CreateTask(Action<ConcurrentQueue<ICmd>> cmd, IJobState? jobState,
        RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null, ITimer? timer = null)
    {
        if (_globalToken.IsCancellationRequested) return null;

        sleepInterval ??= TimeSpan.FromMilliseconds(CONST_defaultSleepInterval);

        var job = new Job<ICmd>(cmd, jobState, opts, sleepInterval, timer);

        Jobs.Add(job.Id, job);

        if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
        {
            if ((opts & RunOptions.UseTimer) == RunOptions.UseTimer &&
                job.Timer != null)
                job.Timer.Start();
            else
                job.Task.Start();
        }

        return job;
    }

    public Job<TCmd> CreateTask<TCmd>(Action<ConcurrentQueue<TCmd>> cmd, IJobState? jobState,
        RunOptions opts = RunOptions.UseSleepInterval, TimeSpan? sleepInterval = null, ITimer? timer = null)
        where TCmd : ICmd
    {
        if (_globalToken.IsCancellationRequested) return null;

        sleepInterval ??= TimeSpan.FromMilliseconds(CONST_defaultSleepInterval);

        var job = new Job<TCmd>(cmd, jobState, opts, sleepInterval, timer);

        Jobs.Add(job.Id, job);

        if ((opts & RunOptions.RunNow) == RunOptions.RunNow)
        {
            if ((opts & RunOptions.UseTimer) == RunOptions.UseTimer &&
                job.Timer != null)
                job.Timer.Start();
            else
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
                if ((opts & RunOptions.UseTimer) == RunOptions.UseTimer &&
                    job.Timer != null)
                    job.Timer.Start();
                else
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

    private void Cleanup()
    {
        var jobs = Jobs.Values
            .SelectMany(j => j.SubJobs)
            .ToList();

        foreach (var job in jobs)
        {
            if (!_expiredStates.Contains(job.Task.Status) ||
                (job.Options & RunOptions.UseTimer) == RunOptions.UseTimer) continue;

            try
            {
                job.Cancel();
            }
            catch
            {
            }

            try
            {
                job.Dispose();
            }
            catch
            {
            }

            if (!Jobs.Remove(job.Id, out var _) &&
                job.ParentId.HasValue &&
                Jobs.TryGetValue(job.ParentId.Value, out var _))
                Jobs[job.ParentId.Value].SubJobs.Remove(job);
        }
    }

    public async Task Run(
        TimeSpan? sleepInterval = null,
        CancellationToken? token = null,
        params IDisposable[] resources)
    {
        if ((resources?.Length ?? 0) > 0) _managedResources.AddRange(resources);

        sleepInterval ??= TimeSpan.FromMilliseconds(CONST_defaultSleepInterval);

        while (!GlobalToken.IsCancellationRequested &&
               token is not { IsCancellationRequested: true })
        {
            Cleanup();

            var jobs = Jobs.Values
                .Union(Jobs.Values
                    .SelectMany(j => j.SubJobs))
                .ToList();
            foreach (var job in jobs)
                if (!_expiredStates.Contains(job.Task.Status) &&
                    job.Task.Status != TaskStatus.Running)
                    try
                    {
                        job.Run();
                    }
                    catch
                    {
                    }

            Task.WaitAny(jobs.Select(j => j.Task).ToArray());

            Cleanup();

            GlobalToken.ThrowIfCancellationRequested();

            if (token.HasValue)
                token.Value.ThrowIfCancellationRequested();

            Thread.Sleep((int)sleepInterval.Value.TotalMilliseconds);
        }
    }

    public void Shutdown()
    {
        Dispose();
    }
}