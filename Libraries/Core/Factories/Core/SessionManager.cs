using System.Collections.Concurrent;
using ThePalace.Common.Factories;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Factories.Core
{
    public partial class SessionManager : SingletonDisposable<SessionManager>
    {
        private readonly ConcurrentDictionary<Guid, ISessionState> _sessions = new();
        public IReadOnlyDictionary<Guid, ISessionState> Sessions => _sessions.AsReadOnly();

        public SessionManager() { }
        ~SessionManager() => this.Dispose(false);

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            _sessions?.Clear();

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        public T CreateSession<T>()
            where T : ISessionState =>
            (T)CreateSession(typeof(T));

        public object CreateSession(Type type)
        {
            if (this.IsDisposed) return null;

            var sessionState = type.GetInstance() as ISessionState;
            if (sessionState == null)
                throw new Exception(string.Format("{0} doesn't implement the ISessionState interface...", type.Name));

            _sessions.TryAdd(sessionState.Id, sessionState);
            return sessionState;
        }

        public void RemoveSession(ISessionState sessionState)
        {
            if (this.IsDisposed) return;

            _sessions.TryRemove(sessionState.Id, out var _);
        }
    }
}