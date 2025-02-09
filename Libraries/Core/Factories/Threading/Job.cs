using ThePalace.Core.Factories.Types;

namespace ThePalace.Core.Factories.Threading
{
    public partial class Job : Tree<Job>, IDisposable
    {
        [Flags]
        public enum JobOptions : int
        {
            None = 0,
            BreakOnError = 0x01,
            RunOnce = 0x02,
            RunUntilCancelled = 0x04,
            UseManualResetEvent = 0x08,
        }

        public partial class RunLog
        {
            public DateTime Start { get; set; }
            public DateTime? Finish { get; set; }
            public DateTime? Error { get; set; }
            public TimeSpan Duration => (Finish ?? Error ?? DateTime.UtcNow).Subtract(Start);
        }

        private Job()
        {
            _token = new();
            _runLogs = new();
            _errors = new();

            Id = Guid.NewGuid();
            IsRunning = false;
            Completions = 0;
            Failures = 0;

            Children = new(Id);
        }

        public Job(Action? cmd = null, JobOptions opts = JobOptions.None) : this()
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            _opts = opts;

            if (JobOptions.UseManualResetEvent.IsBit<JobOptions, int>(_opts))
            {
                _manualResetEvent = new(false);
            }

            _task = Task.Run(_cmd = cmd, _token.Token);
        }

        ~Job() => this.Dispose();

        public void Dispose()
        {
            try { _manualResetEvent?.Dispose(); } catch { }
            try { _token?.Dispose(); } catch { }
            try { Task?.Dispose(); } catch { }

            Children?.ForEach(c => { try { c?.Dispose(); } catch { } });
            Children?.Clear();
            Children?.Dispose();
            Children = null;

            _runLogs?.Clear();
            _runLogs = null;

            _errors?.Clear();
            _errors = null;

            JobState = null;

            GC.SuppressFinalize(this);
        }

        private JobOptions _opts;
        private readonly Action? _cmd;
        private readonly Task _task;
        public Task Task => _task;
        private const int _logLimit = 20;
        private List<RunLog> _runLogs;
        private List<Exception> _errors;

        protected readonly ManualResetEvent _manualResetEvent;
        protected readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;

        public readonly Guid Id;

        public bool IsRunning { get; private set; }
        public int Completions { get; private set; }
        public int Failures { get; private set; }

        public object JobState { get; set; }

        public void Set() => _manualResetEvent?.Set();
        public void Reset() => _manualResetEvent?.Reset();
        public void WaitOne() => _manualResetEvent?.WaitOne();
        public void Cancel() => _token.Cancel();

        public async Task<int> Run()
        {
            var doBreakOnError = JobOptions.BreakOnError.IsBit<JobOptions, int>(_opts);
            var doRunRunOnce =
                JobOptions.RunOnce.IsBit<JobOptions, int>(_opts) &&
                !JobOptions.RunUntilCancelled.IsBit<JobOptions, int>(_opts);
            var doUseManualResetEvent = JobOptions.UseManualResetEvent.IsBit<JobOptions, int>(_opts);

            if (doUseManualResetEvent)
            {
                Reset();
            }

            while (!_token.IsCancellationRequested)
            {
                var runLog = new RunLog
                {
                    Start = DateTime.UtcNow,
                };
                IsRunning = true;

                try
                {
                    _task.Start();

                    runLog.Finish = DateTime.UtcNow;

                    IsRunning = false;
                    Completions++;
                }
                catch (Exception ex)
                {
                    runLog.Error = DateTime.UtcNow;

                    IsRunning = false;
                    Failures++;
                }

                if (_runLogs.Count >= _logLimit)
                    _runLogs.RemoveAt(0);

                _runLogs.Add(runLog);

                if (doRunRunOnce && runLog.Finish.HasValue) return 0;

                if ((doRunRunOnce || doBreakOnError) && runLog.Error.HasValue) return -1;

                if (doUseManualResetEvent)
                {
                    WaitOne();
                }
            }

            return Failures > 0 ? -1 : 0;
        }
    }
}