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
            RunUntilCancelled = 0x02,
            UseManualResetEvent = 0x04,
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

            Id = Guid.NewGuid();
            Counter = 0;
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

            Task = Task.Run(_cmd = cmd);
        }

        ~Job() => this.Dispose();

        public void Dispose()
        {
            Children?.ForEach(c => { try { c?.Dispose(); } catch { } });
            Children?.Clear();
            Children?.Dispose();
            Children = null;

            GC.SuppressFinalize(this);
        }

        private JobOptions _opts;
        private readonly Action? _cmd;
        public readonly Task Task;

        protected readonly ManualResetEvent _manualResetEvent;
        protected readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;

        public readonly Guid Id;

        public int Counter { get; private set; }
        public int Failures { get; private set; }

        public object JobState { get; set; }

        private const int _logLimit = 20;
        private List<RunLog> _runLogs;
        public int Run()
        {
            Counter++;

            var doBreakOnError = JobOptions.BreakOnError.IsBit<JobOptions, int>(_opts);
            var doRunUntilCancelled = JobOptions.RunUntilCancelled.IsBit<JobOptions, int>(_opts);
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

                try
                {
                    _cmd();

                    runLog.Finish = DateTime.UtcNow;

                    if (!doRunUntilCancelled) return 0;
                }
                catch (Exception ex)
                {
                    runLog.Error = DateTime.UtcNow;

                    Failures++;

                    if (!doRunUntilCancelled ||
                        doBreakOnError) return -1;
                }

                if (_runLogs.Count >= _logLimit)
                    _runLogs.RemoveAt(0);

                _runLogs.Add(runLog);

                if (doUseManualResetEvent)
                {
                    WaitOne();
                }
            }

            return Failures > 0 ? -1 : 0;
        }

        public void Cancel() => _token.Cancel();

        public void Set() => _manualResetEvent?.Set();
        public void Reset() => _manualResetEvent?.Reset();
        public void WaitOne() => _manualResetEvent?.WaitOne();
    }
}