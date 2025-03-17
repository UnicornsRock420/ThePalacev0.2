using System.Collections.Concurrent;
using Lib.Core.Interfaces.Core;

namespace Lib.Core.Factories.Core;

public class SessionManager : SingletonDisposable<SessionManager>
{
    private readonly ConcurrentDictionary<Guid, ISessionState<IApp>> _sessions = new();
    public IReadOnlyDictionary<Guid, ISessionState<IApp>> Sessions => _sessions.AsReadOnly();

    ~SessionManager()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        _sessions?.Clear();

        base.Dispose();
    }

    public TSessionState? CreateSession<TSessionState, TApp>(TApp app)
        where TSessionState : ISessionState<TApp>
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

        if (type.GetInstance() is not ISessionState<IApp> sessionState)
            throw new Exception($"{type.Name} doesn't implement the ISessionState interface...");
        
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