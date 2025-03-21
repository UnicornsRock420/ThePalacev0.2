using System.Collections.Concurrent;
using Lib.Core.Interfaces.Core;

namespace Lib.Core.Singletons;

public class SessionManager : Singleton<SessionManager>
{
    private bool IsDisposed { get; set; }
    private readonly ConcurrentDictionary<Guid, ISessionState> _sessions = new();
    public IReadOnlyDictionary<Guid, ISessionState> Sessions => _sessions.AsReadOnly();

    ~SessionManager()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;

        _sessions?.Clear();

        GC.SuppressFinalize(this);
    }

    public TSessionState? CreateSession<TSessionState, TApp>(TApp app)
        where TSessionState : ISessionState
        where TApp : IApp
    {
        if (IsDisposed) return default(TSessionState);

        ArgumentNullException.ThrowIfNull(app, nameof(app));

        return (TSessionState)CreateSession(typeof(TSessionState), app);
    }

    public object? CreateSession(Type type, IApp app)
    {
        if (IsDisposed) return null;

        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        var sessionState = type.GetInstance() as ISessionState;
        sessionState.App = app;

        _sessions.TryAdd(sessionState.Id, sessionState);
        return sessionState;
    }

    public void RemoveSession(ISessionState sessionState)
    {
        if (IsDisposed) return;

        _sessions.TryRemove(sessionState.Id, out _);
    }
}