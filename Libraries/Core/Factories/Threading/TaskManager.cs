using ThePalace.Core.Interfaces.Core;
using static ThePalace.Core.Factories.Threading.Job;

namespace ThePalace.Core.Factories.Threading
{
    public partial class TaskManager : Singleton<TaskManager>, IDisposable
    {
        static TaskManager()
        {
            _globalToken = CancellationTokenFactory.NewToken();
        }

        public TaskManager()
        {
            _resetEvent = new(false);
            _jobs = new();
        }

        ~TaskManager() => Dispose();

        public void Dispose()
        {
            if (_globalToken.IsCancellationRequested == false)
            {
                try { _globalToken.Cancel(); } catch { }

                _jobs.Values
                    .SelectMany(j => j._subJobs)
                    .ToList()
                    .ForEach(j =>
                    {
                        try { j.Cancel(); } catch { }
                        try { j.Dispose(); } catch { }

                        j.JobState = null;
                    });
                _jobs.Clear();

                Thread.Sleep(1500);
            }

            _globalToken.Dispose();
        }

        private static readonly CancellationTokenSource _globalToken;

        private readonly ManualResetEvent _resetEvent;
        private readonly Dictionary<Guid, Job> _jobs;
        public IReadOnlyDictionary<Guid, Job> Jobs => _jobs.AsReadOnly();

        public static CancellationToken GlobalToken => _globalToken.Token;

        public Task CreateTask(Action cmd, IJobState? jobState, RunOptions opts = RunOptions.UseSleepInterval)
        {
            if (_globalToken.IsCancellationRequested) return null;

            var job = new Job(cmd, jobState, opts);

            _jobs.Add(job.Id, job);

            if (RunOptions.RunNow.IsSet<RunOptions, int>(opts))
            {
                job.Task.Start();
            }

            return job.Task;
        }

        public static async Task Fork(Job parent, int threadCount = 1, RunOptions opts = RunOptions.RunNow)
        {
            if (_globalToken.IsCancellationRequested ||
                threadCount < 1) return;

            for (var j = threadCount; j > 0; j--)
            {
                var job = new Job(parent.Cmd, parent.JobState, parent.Options);
                parent._subJobs.Add(job);

                if (RunOptions.RunNow.IsSet<RunOptions, int>(opts))
                {
                    job.Task.Start();
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
            var jobs = _jobs.Values.ToList();

            foreach (var job in jobs)
            {
                if (_expiredStates.Contains(job.Task.Status))
                {
                    try { job.Cancel(); } catch { }
                    try { job.Dispose(); } catch { }

                    _jobs.Remove(job.Id);
                }
            }
        }

        public void Run(int sleepInterval = 1000, CancellationToken? token = null)
        {
            while (!GlobalToken.IsCancellationRequested &&
                !(!token.HasValue || token.Value.IsCancellationRequested))
            {
                Cleanup();

                var jobs = _jobs.Values.ToList();
                foreach (var job in jobs)
                {
                    if (!_expiredStates.Contains(job.Task.Status) &&
                        job.Task.Status != TaskStatus.Running)
                    {
                        try { job.Task.Start(); } catch { }
                    }
                }

                var index = Task.WaitAny(jobs.Select(j => j.Task).ToArray());
                if (index > -1)
                {
                    var _job = jobs[index];

                    if (_expiredStates.Contains(_job.Task.Status))
                    {
                        try { _job.Cancel(); } catch { }
                        try { _job.Dispose(); } catch { }

                        _jobs.Remove(_job.Id);

                        jobs = _jobs.Values.ToList();
                    }
                }

                Cleanup();

                GlobalToken.ThrowIfCancellationRequested();

                if (token.HasValue)
                    token.Value.ThrowIfCancellationRequested();

                Thread.Sleep(sleepInterval);
            }
        }

        public void Shutdown() => Dispose();
    }
}