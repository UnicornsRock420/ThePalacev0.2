using ThePalace.Core.Factories.Types;

namespace ThePalace.Core.Factories.Threading
{
    public partial class TaskManager : Singleton<TaskManager>, IDisposable
    {
        static TaskManager()
        {
            _globalToken = new();
        }

        public TaskManager()
        {
            _tokens = new();
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
        private Root<CancellationTokenSource> _tokens;
        private readonly List<Job> _jobs;

        public static CancellationToken GlobalToken => _globalToken.Token;

        public Task CreateTask(Action cmd, bool start = true)
        {
            var job = new Job(cmd);

            if (start)
            {
                job.Task.Start();
            }

            return job.Task;
        }

        public void Shutdown()
        {
            try { _globalToken.Cancel(); } catch { }
        }
    }
}