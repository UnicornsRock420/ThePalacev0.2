﻿using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Factories.Threading
{
    public partial class Job : IDisposable
    {
        [Flags]
        public enum JobOptions : int
        {
            None = 0,
            BreakOnError = 0x01,
            RunNow = 0x02,
            RunOnce = 0x04,
            UseSleepInterval = 0x08,
            UseManualResetEvent = 0x10,
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

        public Job(Action? cmd = null, IJobState? jobState = null, JobOptions opts = JobOptions.UseSleepInterval) : this()
        {
            if (cmd == null) throw new ArgumentNullException(nameof(cmd));

            JobState = jobState;
            Options = opts;

            if (JobOptions.UseManualResetEvent.IsSet<JobOptions, int>(Options))
            {
                _manualResetEvent = new(false);
            }

            Task = Task
                .Run(
                    Cmd = cmd,
                    _token.Token)
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

        public void Dispose()
        {
            try { _manualResetEvent?.Dispose(); } catch { }
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
        public readonly Task Task;

        private const int _CONST_INT_LOGLIMIT = 20;
        private List<RunLog> _runLogs;
        public IReadOnlyList<RunLog> RunLogs => _runLogs.AsReadOnly();

        private const int _CONST_INT_ERRORLIMIT = 20;
        private List<Exception> _errors;
        public IReadOnlyList<Exception> Errors => _errors.AsReadOnly();

        protected readonly CancellationTokenSource _token;
        public CancellationToken Token => _token.Token;
        private readonly List<Job> _subJobs;
        public IReadOnlyList<Job> SubJobs => _subJobs.AsReadOnly();

        public JobOptions Options { get; set; }
        public readonly Guid Id;
        public bool IsRunning { get; protected set; }
        public int Completions { get; protected set; }
        public int Failures { get; protected set; }
        protected readonly ManualResetEvent _manualResetEvent;
        public TimeSpan SleepInterval { get; set; }
        public IJobState? JobState { get; set; }

        public void Set() => _manualResetEvent?.Set();
        public void Reset() => _manualResetEvent?.Reset();
        public void WaitOne() => _manualResetEvent?.WaitOne();
        public void Cancel() => _token.Cancel();

        public async Task<int> Run()
        {
            var doBreakOnError = JobOptions.BreakOnError.IsSet<JobOptions, int>(Options);
            var doRunRunOnce = JobOptions.RunOnce.IsSet<JobOptions, int>(Options);
            var doUseSleepInterval = JobOptions.UseSleepInterval.IsSet<JobOptions, int>(Options);
            var doUseManualResetEvent = JobOptions.UseManualResetEvent.IsSet<JobOptions, int>(Options);

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

        public async Task Fork(int threadCount = 1)
        {
            if (threadCount < 1) return;

            for (var j = threadCount; j > 0; j--)
            {
                var subJob = new Job(Cmd, JobState, Options);
                _subJobs.Add(subJob);
            }
        }
    }
}