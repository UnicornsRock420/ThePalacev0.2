﻿using System.Collections.Concurrent;
using Lib.Common.Interfaces.Core;
using Lib.Common.Threading;

namespace Lib.Common.Interfaces.Threading;

public interface IJob : IDisposable, IID
{
    Task Task { get; }

    List<RunLog> RunLogs { get; }

    List<Exception> Errors { get; }

    CancellationTokenSource TokenSource { get; }
    CancellationToken Token => TokenSource.Token;

    DisposableList<IJob> SubJobs { get; }

    RunOptions Options { get; set; }
    Guid? ParentId { get; }
    bool IsRunning { get; }
    int Completions { get; }
    int Failures { get; }
    ManualResetEvent ResetEvent { get; }
    TimeSpan SleepInterval { get; set; }
    ITimer Timer { get; set; }

    IJobState? JobState { get; set; }

    void Cancel(CancelOptions opts = CancelOptions.Cascade);
    Task<int> Run();
}

public interface IJob<TCmd> : IJob, IDisposable
    where TCmd : class, ICmd
{
    Action<ConcurrentQueue<TCmd>> Cmd { get; }
    new DisposableList<IJob<TCmd>> SubJobs { get; }

    ConcurrentQueue<TCmd> Queue { get; }
    void Build(Action<ConcurrentQueue<TCmd>>? cmd = null, CancellationToken? token = null);
    void Enqueue(TCmd cmd, bool clear);
}