using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Common.Threading
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
            OnlyMain = 0x01,
            OnlyChildren = 0x02,
            Cascade = OnlyMain | OnlyChildren,
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

            if ((Options & RunOptions.UseManualResetEvent) == RunOptions.UseManualResetEvent)
            {
                _resetEvent = new(false);
            }

            Build(Cmd = cmd);
        }

        internal Job(Job parent) : this(parent.Cmd, parent.JobState, parent.Options)
        {
            ParentId = parent.Id;
            Cmd = parent.Cmd;
            JobState = parent.JobState;
            Options = parent.Options;
        }

        ~Job() => Dispose();

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

        private const int _CONST_INT_ERRORLIMIT = 50;
        private List<Exception> _errors;
        public IReadOnlyList<Exception> Errors => _errors.AsReadOnly();

        internal readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;
        internal readonly List<Job> _subJobs;
        public IReadOnlyList<Job> SubJobs => _subJobs.AsReadOnly();

        public RunOptions Options { get; set; }
        public readonly Guid Id;
        public readonly Guid? ParentId;
        public bool IsRunning { get; protected set; }
        public int Completions { get; protected set; }
        public int Failures { get; protected set; }
        internal readonly ManualResetEvent _resetEvent;
        public TimeSpan SleepInterval { get; set; }
        public IJobState? JobState { get; set; }

        public void EventSet() => _resetEvent?.Set();
        public void EventReset() => _resetEvent?.Reset();
        public void EventWaitOne() => _resetEvent?.WaitOne();

        public void Cancel(CancelOptions opts = CancelOptions.Cascade)
        {
            var doCascade = (opts & CancelOptions.Cascade) == CancelOptions.Cascade;
            var jobs = new List<Job>();

            if (doCascade || (opts & CancelOptions.OnlyMain) == CancelOptions.OnlyMain)
            {
                jobs.Add(this);
            }

            if (doCascade || (opts & CancelOptions.OnlyChildren) == CancelOptions.OnlyChildren)
            {
                jobs.AddRange(_subJobs
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
            var doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
            var doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
            var doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
            var doUseManualResetEvent = (Options & RunOptions.UseManualResetEvent) == RunOptions.UseManualResetEvent;

            if (doUseManualResetEvent)
            {
                EventReset();
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
                    EventWaitOne();
                }
                else if (doUseSleepInterval)
                {
                    var timer = DateTime.UtcNow;

                    await Task.Delay(SleepInterval);

                    if (DateTime.UtcNow.Subtract(timer).TotalMilliseconds < SleepInterval.TotalMilliseconds * 0.70)
                    {
                        Thread.Sleep(SleepInterval);
                    }
                }

                doBreakOnError = (Options & RunOptions.BreakOnError) == RunOptions.BreakOnError;
                doRunRunOnce = (Options & RunOptions.RunOnce) == RunOptions.RunOnce;
                doUseSleepInterval = (Options & RunOptions.UseSleepInterval) == RunOptions.UseSleepInterval;
                doUseManualResetEvent = (Options & RunOptions.UseManualResetEvent) == RunOptions.UseManualResetEvent;
            }

            return Failures > 0 ? -1 : 0;
        }
    }
}