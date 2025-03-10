using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Logging.Entities;

namespace ThePalace.Client.Desktop.Factories;

public class ScriptEvents : SingletonDisposable<ScriptEvents>
{
    private static readonly IReadOnlyList<IptEventTypes> _eventTypes = Enum.GetValues<IptEventTypes>().ToList();

    private ConcurrentDictionary<IptEventTypes, List<EventHandler>> _events = new();

    public ScriptEvents()
    {
        foreach (var type in _eventTypes)
            _events[type] = [];
    }

    ~ScriptEvents()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        foreach (var @event in _events.Values)
            try
            {
                @event?.Clear();
            }
            catch
            {
            }

        _events.Clear();
        _events = null;

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public void Invoke(IptEventTypes eventType, IDesktopSessionState sessionState, IProtocol packet,
        object scriptState = null)
    {
        var scriptEvent = new ScriptEvent
        {
            EventType = eventType,
            Packet = packet,
            ScriptState = scriptState
        };

        foreach (var handler in _events[eventType])
            try
            {
                handler(sessionState, scriptEvent);
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);

                if (eventType != IptEventTypes.UnhandledError)
                    Invoke(IptEventTypes.UnhandledError, sessionState, packet, sessionState.ScriptState);
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