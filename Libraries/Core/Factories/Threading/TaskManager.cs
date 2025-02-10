﻿using ThePalace.Core.Interfaces.Core;
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
            var job = new Job(cmd, jobState, opts);

            _jobs.Add(job.Id, job);

            if (RunOptions.RunNow.IsSet<RunOptions, int>(opts) &&
                !job.Token.IsCancellationRequested &&
                !job.Task.IsCanceled)
            {
                job.Task.Start();
            }

            return job.Task;
        }

        public static async Task Fork(Job parent, int threadCount = 1, RunOptions opts = RunOptions.RunNow)
        {
            if (threadCount < 1) return;

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

        public void Run(int sleepInterval = 750)
        {
            while (!GlobalToken.IsCancellationRequested)
            {
                Task.WaitAny(_jobs.Values.Select(j => j.Task).ToArray());

                foreach (var job in _jobs.Values.ToList())
                {
                    if (job.Task.Status != TaskStatus.WaitingForActivation &&
                        job.Task.Status != TaskStatus.Running)
                    {
                        try { job.Dispose(); } catch { }

                        _jobs.Remove(job.Id);
                    }
                }

                Thread.Sleep(sleepInterval);

                GlobalToken.ThrowIfCancellationRequested();
            }
        }

        public void Shutdown() => Dispose();
    }
}