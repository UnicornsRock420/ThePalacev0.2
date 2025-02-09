using ThePalace.Core.Factories.Types;

namespace ThePalace.Core.Factories.Threading
{
    public partial class Job : IDisposable
    {
        private Job() { }

        public Job(Action? cmd = null)
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            _cmd = cmd;
        }

        ~Job() => this.Dispose();

        public void Dispose() { }

        private readonly Action _cmd = null;

        public int Counter { get; private set; } = 0;
        public int Failures { get; private set; } = 0;

        public int Run()
        {
            Counter++;

            try
            {
                _cmd();

                return 0;
            }
            catch (Exception ex)
            {
                Failures++;

                return -1;
            }
        }
    }

    public partial class TaskManager : Singleton<TaskManager>, IDisposable
    {
        static TaskManager()
        {
            _globalToken = new();
        }

        public TaskManager()
        {
            _subTokens = new();
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
        private Root<Guid, CancellationTokenSource> _subTokens;
        private readonly List<Task> _jobs;

        public static CancellationToken GlobalToken => _globalToken.Token;

        public Task CreateTask(Action cmd)
        {
            var task = Task.Factory.StartNew(cmd);

            return task;
        }

        public void Shutdown()
        {
            try { _globalToken.Cancel(); } catch { }
        }
    }
}