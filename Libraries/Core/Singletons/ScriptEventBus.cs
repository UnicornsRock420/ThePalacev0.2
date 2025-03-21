using System.Collections.Concurrent;
using Lib.Core.Entities.Scripting;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.Network;
using Lib.Logging.Entities;

namespace Lib.Core.Singletons;

public class ScriptEventBus : Singleton<ScriptEventBus>, IDisposable
{
    private bool IsDisposed { get; set; }
    private static readonly IReadOnlyList<ScriptEventTypes> _eventTypes = Enum.GetValues<ScriptEventTypes>().ToList();
    private ConcurrentDictionary<ScriptEventTypes, List<EventHandler>> _events = new();

    public ScriptEventBus()
    {
        foreach (var type in _eventTypes)
            _events[type] = [];
    }

    ~ScriptEventBus()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (IsDisposed) return;

        IsDisposed = true;

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

        GC.SuppressFinalize(this);
    }

    public void Invoke(
        ScriptEventTypes eventType,
        IUserSessionState sessionState,
        IProtocol packet,
        object scriptState = null)
    {
        if (IsDisposed) return;

        var scriptEvent = new ScriptEvent
        {
            EventType = (int)eventType,
            Msg = packet,
            ScriptTag = scriptState
        };

        foreach (var handler in _events[eventType])
            try
            {
                handler(sessionState, scriptEvent);
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);

                if (eventType != ScriptEventTypes.UnhandledError)
                    Invoke(
                        ScriptEventTypes.UnhandledError,
                        sessionState,
                        packet,
                        sessionState.ScriptTag);
            }
    }

    public void RegisterEvent(
        ScriptEventTypes eventType,
        EventHandler handler)
    {
        if (IsDisposed) return;

        if (handler != null)
            _events[eventType].Add(handler);
    }

    public void UnregisterEvent(
        ScriptEventTypes eventType,
        EventHandler handler)
    {
        if (IsDisposed) return;

        _events[eventType].Remove(handler);
    }

    public void ClearEvents(ScriptEventTypes eventType)
    {
        if (IsDisposed) return;

        _events[eventType].Clear();
    }
}