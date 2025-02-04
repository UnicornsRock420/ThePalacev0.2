using System.Collections.Concurrent;
using System.Diagnostics;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Factories
{
    public class ScriptEvents : Disposable
    {
        private static readonly IReadOnlyList<IptEventTypes> _eventTypes = Enum.GetValues<IptEventTypes>().ToList();

        private static readonly Lazy<ScriptEvents> _current = new();
        public static ScriptEvents Current => _current.Value;

        private ConcurrentDictionary<IptEventTypes, List<EventHandler>> _events = new();

        public ScriptEvents()
        {
            foreach (var type in _eventTypes)
                _events[type] = [];
        }
        ~ScriptEvents() =>
            Dispose(false);

        public override void Dispose()
        {
            if (IsDisposed) return;

            base.Dispose();

            foreach (var @event in _events.Values)
                try { @event?.Clear(); } catch { }
            _events.Clear();
            _events = null;
        }

        public void Invoke(IptEventTypes eventType, ISessionState sessionState, IStruct packet, object? scriptState = null)
        {
            var scriptEvent = new ScriptEventArgs
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
                        Invoke(IptEventTypes.UnhandledError, sessionState, packet, sessionState.ScriptState);
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