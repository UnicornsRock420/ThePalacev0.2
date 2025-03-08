using System.Collections.Concurrent;
using ThePalace.Common.Factories;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Factories.Core;

public class SessionManager : SingletonDisposable<SessionManager>
{
    private readonly ConcurrentDictionary<Guid, ISessionState> _sessions = new();
    public IReadOnlyDictionary<Guid, ISessionState> Sessions => _sessions.AsReadOnly();

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

    public T CreateSession<T>()
        where T : ISessionState
    {
        return (T)CreateSession(typeof(T));
    }

    public object CreateSession(Type type)
    {
        if (IsDisposed) return null;

        if (type.GetInstance() is not ISessionState sessionState)
            throw new Exception(string.Format("{0} doesn't implement the ISessionState interface...", type.Name));

        _sessions.TryAdd(sessionState.Id, sessionState);
        return sessionState;
    }

    public void RemoveSession(ISessionState sessionState)
    {
        if (IsDisposed) return;

        _sessions.TryRemove(sessionState.Id, out _);
    }
}