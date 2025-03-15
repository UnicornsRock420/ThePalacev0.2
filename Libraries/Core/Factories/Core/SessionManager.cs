using System.Collections.Concurrent;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Factories.Core;

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

    public TSessionState CreateSession<TSessionState, TApp>()
        where TSessionState : ISessionState<TApp>
        where TApp : IApp
    {
        return (TSessionState)CreateSession(typeof(TSessionState));
    }

    public object CreateSession(Type type)
    {
        if (IsDisposed) return null;

        if (type.GetInstance() is not ISessionState<IApp> sessionState)
            throw new Exception($"{type.Name} doesn't implement the ISessionState interface...");

        _sessions.TryAdd(sessionState.Id, sessionState);
        return sessionState;
    }

    public void RemoveSession(ISessionState sessionState)
    {
        if (IsDisposed) return;

        _sessions.TryRemove(sessionState.Id, out _);
    }
}