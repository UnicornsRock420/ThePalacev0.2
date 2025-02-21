using System.Collections.Concurrent;
using System.Diagnostics;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Common.Factories;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Client.Desktop.Factories
{
    public class ScriptEvents : SingletonDisposable<ScriptEvents>
    {
        private static readonly IReadOnlyList<IptEventTypes> _eventTypes = Enum.GetValues<IptEventTypes>().ToList();

        private static readonly Lazy<ScriptEvents> _current = new();
        public static ScriptEvents Current => _current.Value;

        private ConcurrentDictionary<IptEventTypes, List<EventHandler>> _events = new();

        public ScriptEvents()
        {
            foreach (var type in _eventTypes)
                _events[type] = new List<EventHandler>();
        }
        ~ScriptEvents() => Dispose(false);

        public override void Dispose()
        {
            if (IsDisposed) return;

            base.Dispose();

            foreach (var @event in _events.Values)
                try { @event?.Clear(); } catch { }
            _events.Clear();
            _events = null;
        }

        public void Invoke(IptEventTypes eventType, ISessionState sessionState, IProtocol packet, object scriptState = null)
        {
            var scriptEvent = new ScriptEvent
            {
                EventType = eventType,
                Packet = packet,
                ScriptState = scriptState,
            };

            foreach (var handler in _events[eventType])
            {
                try
                {
                    handler(sessionState, scriptEvent);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Debug.WriteLine(ex.Message);
#endif

                    if (eventType != IptEventTypes.UnhandledError)
                        this.Invoke(IptEventTypes.UnhandledError, sessionState, packet, sessionState.State);
                }
            }
        }

        public void RegisterEvent(IptEventTypes eventType, EventHandler handler)
        {
            if (handler != null)
                _events[eventType].Add(handler);
        }

        public void UnregisterEvent(IptEventTypes eventType, EventHandler handler)
        {
            _events[eventType].Remove(handler);
        }
    }
}