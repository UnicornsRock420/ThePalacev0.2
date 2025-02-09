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
        private readonly Root<Job> _jobs;

        public static CancellationToken GlobalToken => _globalToken.Token;

        public Task CreateTask(Action cmd, object jobState, Job.JobOptions opts = Job.JobOptions.UseSleepInterval)
        {
            var job = new Job(cmd, jobState, opts);

            var xPath = job.XPath;
            xPath.Add(job.Id);

            _jobs.Journal.Add(job.Id, xPath);
            _jobs.Children.Add(job);

            if (Job.JobOptions.RunNow.IsSet<JobOptions, int>(opts))
            {
                job.Task.Start();
            }

            return job.Task;
        }

        public bool Cancel(Guid jobId, bool cascade = false)
        {
            if (!_jobs.Journal.ContainsKey(jobId)) return false;

            var jobs = new List<Job>();

            if (cascade)
            {
                var xPath = _jobs.Journal[jobId];

                var node = GetNode(_jobs.Children, xPath);

                jobs.AddRange(node.SelectMany(n => n.Children));
            }

            jobs.ForEach(j => j.Cancel());

            return true;
        }

        public static Tree<Job> GetNode(Tree<Job> node, List<Guid> xPath)
        {
            var result = (Tree<Job>?)node;

            foreach (var id in xPath)
            {
                if (result == null ||
                    result.Count < 1) break;

                foreach (var job in result.Children)
                {
                    if (job.Id == id)
                    {
                        result = result.Children;

                        break;
                    }
                }
            }

            return result;
        }

        public void Shutdown()
        {
            try { _globalToken.Cancel(); } catch { }
        }
    }
}