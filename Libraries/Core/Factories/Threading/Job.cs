using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Factories.Threading
{
    public partial class Job : IDisposable
    {
        [Flags]
        public enum RunOptions : int
        {
            None = 0,
            BreakOnError = 0x01,
            RunNow = 0x02,
            RunOnce = 0x04,
            UseSleepInterval = 0x08,
            UseManualResetEvent = 0x10,
        }

        [Flags]
        public enum CancelOptions : int
        {
            Cascade = 0x01,
            OnlyChildren = 0x02,
        }

        public partial class RunLog
        {
            public DateTime Start { get; set; }
            public DateTime? Finish { get; set; }
            public DateTime? Error { get; set; }
            public Exception? Exception { get; set; }
            public TimeSpan Duration => (Finish ?? Error ?? DateTime.UtcNow).Subtract(Start);
        }

        private Job()
        {
            _token = CancellationTokenFactory.NewToken();
            _subJobs = new();
            _runLogs = new();
            _errors = new();

            Id = Guid.NewGuid();

            IsRunning = false;

            Completions = 0;
            Failures = 0;

            SleepInterval = TimeSpan.FromMilliseconds(1500);
            JobState = null;
        }

        public Job(Action? cmd = null, IJobState? jobState = null, RunOptions opts = RunOptions.UseSleepInterval) : this()
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            JobState = jobState;
            Options = opts;

            if (RunOptions.UseManualResetEvent.IsSet<RunOptions, int>(Options))
            {
                _resetEvent = new(false);
            }

            Build(Cmd = cmd);
        }

        public Job(Job src) : this()
        {
            Cmd = src.Cmd;
            JobState = src.JobState;
            Options = src.Options;
        }

        ~Job() => this.Dispose();

        public void Dispose()
        {
            try { _resetEvent?.Dispose(); } catch { }
            try { _token?.Dispose(); } catch { }
            try { Task?.Dispose(); } catch { }

            _subJobs?.ForEach(j => { try { j?.Dispose(); } catch { } });
            _subJobs?.Clear();

            _runLogs?.ForEach(l => { try { l.Exception = null; } catch { } });
            _runLogs?.Clear();
            _runLogs = null;

            _errors?.Clear();
            _errors = null;

            JobState = null;

            GC.SuppressFinalize(this);
        }

        internal readonly Action Cmd;
        internal Task Task;

        private const int _CONST_INT_LOGLIMIT = 20;
        private List<RunLog> _runLogs;
        public IReadOnlyList<RunLog> RunLogs => _runLogs.AsReadOnly();

        private const int _CONST_INT_ERRORLIMIT = 20;
        private List<Exception> _errors;
        public IReadOnlyList<Exception> Errors => _errors.AsReadOnly();

        internal readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;
        internal readonly List<Job> _subJobs;
        public IReadOnlyList<Job> SubJobs => _subJobs.AsReadOnly();

        public RunOptions Options { get; set; }
        public readonly Guid Id;
        public bool IsRunning { get; protected set; }
        public int Completions { get; protected set; }
        public int Failures { get; protected set; }
        internal readonly ManualResetEvent _resetEvent;
        public TimeSpan SleepInterval { get; set; }
        public IJobState? JobState { get; set; }

        public void Set() => _resetEvent?.Set();
        public void Reset() => _resetEvent?.Reset();
        public void WaitOne() => _resetEvent?.WaitOne();

        public void Cancel(CancelOptions opts = CancelOptions.OnlyChildren)
        {
            var jobs = new List<Job>();

            if (CancelOptions.Cascade.IsSet<CancelOptions, int>(opts) ||
                !CancelOptions.OnlyChildren.IsSet<CancelOptions, int>(opts))
            {
                jobs.Add(this);
            }

            if (CancelOptions.Cascade.IsSet<CancelOptions, int>(opts))
            {
                jobs.AddRange(this._subJobs
                    .SelectMany(j => j._subJobs));
            }

            jobs.ForEach(t => t.Cancel());
        }

        public void Build(Action? cmd = null, CancellationToken? token = null)
        {
            if (Task != null)
            {
                try { Cancel(); } catch { }
                try { Task?.Dispose(); } catch { }
            }

            Task = new Task(cmd ?? Cmd, token ?? _token.Token);
        }

        public async Task<int> Run()
        {
            var doBreakOnError = RunOptions.BreakOnError.IsSet<RunOptions, int>(Options);
            var doRunRunOnce = RunOptions.RunOnce.IsSet<RunOptions, int>(Options);
            var doUseSleepInterval = RunOptions.UseSleepInterval.IsSet<RunOptions, int>(Options);
            var doUseManualResetEvent = RunOptions.UseManualResetEvent.IsSet<RunOptions, int>(Options);

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
                    Task.Start();

                    runLog.Finish = DateTime.UtcNow;

                    IsRunning = false;
                    Completions++;
                }
                catch (Exception ex)
                {
                    runLog.Error = DateTime.UtcNow;

                    IsRunning = false;
                    Failures++;

                    if (_errors.Count >= _CONST_INT_LOGLIMIT)
                        _errors.RemoveAt(0);

                    _errors.Add(runLog.Exception = ex);
                }

                if (_runLogs.Count >= _CONST_INT_ERRORLIMIT)
                    _runLogs.RemoveAt(0);

                _runLogs.Add(runLog);

                if (doRunRunOnce && runLog.Finish.HasValue) return 0;

                if ((doRunRunOnce || doBreakOnError) && runLog.Error.HasValue) return -1;

                Token.ThrowIfCancellationRequested();

                if (doUseManualResetEvent)
                {
                    WaitOne();
                }
                else if (doUseSleepInterval)
                {
                    var timer = DateTime.UtcNow;

                    await Task.Delay(SleepInterval);

                    if (DateTime.UtcNow.Subtract(timer).TotalMilliseconds < (SleepInterval.TotalMilliseconds * 0.70))
                    {
                        Thread.Sleep(SleepInterval);
                    }
                }
            }

            return Failures > 0 ? -1 : 0;
        }
    }
}