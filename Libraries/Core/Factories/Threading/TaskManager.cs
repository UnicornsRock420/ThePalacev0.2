using ThePalace.Core.Factories.Types;
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
            _jobs = new();
        }

        ~TaskManager() => Dispose();

        public void Dispose()
        {
            if (_globalToken.IsCancellationRequested == false)
            {
                Shutdown();

                Thread.Sleep(1500);
            }

            _globalToken.Dispose();
        }

        private static readonly CancellationTokenSource _globalToken;
        private readonly Dictionary<Guid, Job> _jobs;
        public IReadOnlyDictionary<Guid, Job> Jobs => _jobs.AsReadOnly();

        public static CancellationToken GlobalToken => _globalToken.Token;

        public Task CreateTask(Action cmd, object jobState, Job.JobOptions opts = Job.JobOptions.UseSleepInterval)
        {
            var job = new Job(cmd, jobState, opts);

            _jobs.Add(job.Id, job);

            if (Job.JobOptions.RunNow.IsSet<JobOptions, int>(opts))
            {
                job.Task.Start();
            }

            return job.Task;
        }

        public bool Cancel(Guid jobId, bool cascade = true)
        {
            if (!_jobs.ContainsKey(jobId)) return false;

            var jobs = new List<Job>{ _jobs[jobId] };

            if (cascade)
            {
                jobs.AddRange(
                    _jobs[jobId].SubJobs
                        .SelectMany(n => n.SubJobs ?? []));
            }

            jobs.ForEach(j => j.Cancel());

            return true;
        }

        public void Shutdown()
        {
            try { _globalToken.Cancel(); } catch { }
        }
    }
}