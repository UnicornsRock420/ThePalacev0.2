﻿using ThePalace.Core.Factories.Types;

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
            UseManualResetEvent = 0x04,
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
            _token = new();
            _runLogs = new();
            _errors = new();

            Id = Guid.NewGuid();

            IsRunning = false;

            Completions = 0;
            Failures = 0;

            JobState = null;
        }

        public Job(Action? cmd = null, object? jobState = null, JobOptions opts = JobOptions.None) : this()
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            JobState = jobState;
            _opts = opts;

            if (JobOptions.UseManualResetEvent.IsBit<JobOptions, int>(_opts))
            {
                _manualResetEvent = new(false);
            }

            _task = Task
                .Run(_cmd = cmd, _token.Token)
                .ContinueWith(
                    t =>
                    {
                        t.Exception?.Handle(e =>
                            e is OperationCanceledException &&
                            _token.IsCancellationRequested);
#if DEBUG
                        Console.WriteLine("You have canceled the task");
#endif
                    },
                    TaskContinuationOptions.OnlyOnCanceled);
        }

        ~Job() => this.Dispose();

        public override void Dispose()
        {
            try { _manualResetEvent?.Dispose(); } catch { }
            try { _token?.Dispose(); } catch { }
            try { Task?.Dispose(); } catch { }

            _runLogs?.ForEach(l => { try { l.Exception = null; } catch { } });
            _runLogs?.Clear();
            _runLogs = null;

            _errors?.Clear();
            _errors = null;

            JobState = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        private JobOptions _opts;
        private readonly Action? _cmd;
        private readonly Task _task;
        public Task Task => _task;

        private const int _CONST_INT_LOGLIMIT = 20;
        private List<RunLog> _runLogs;
        public IReadOnlyList<RunLog> RunLogs => _runLogs.AsReadOnly();

        private const int _CONST_INT_ERRORLIMIT = 20;
        private List<Exception> _errors;
        public IReadOnlyList<Exception> Errors => _errors.AsReadOnly();

        protected readonly ManualResetEvent _manualResetEvent;
        protected readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;

        public readonly Guid Id;
        public int SleepInterval { get; set; } = 1500;
        public bool IsRunning { get; protected set; }
        public int Completions { get; protected set; }
        public int Failures { get; protected set; }
        public object? JobState { get; set; }


        public void Set() => _manualResetEvent?.Set();
        public void Reset() => _manualResetEvent?.Reset();
        public void WaitOne() => _manualResetEvent?.WaitOne();
        public void Cancel() => _token.Cancel();

        public async Task<int> Run()
        {
            var doBreakOnError = JobOptions.BreakOnError.IsBit<JobOptions, int>(_opts);
            var doRunRunOnce = JobOptions.RunOnce.IsBit<JobOptions, int>(_opts);
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
                    runLog.Exception = ex;

                    IsRunning = false;
                    Failures++;

                    if (_errors.Count >= _CONST_INT_LOGLIMIT)
                        _errors.RemoveAt(0);

                    _errors.Add(ex);
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
                else
                {
                    Thread.Sleep(SleepInterval);
                }
            }

            return Failures > 0 ? -1 : 0;
        }
    }
}