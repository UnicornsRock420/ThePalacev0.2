using System.Collections.Concurrent;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.Network;
using Lib.Logging.Entities;

namespace Lib.Core.Singletons;

public class ScriptEventBus : Singleton<ScriptEventBus>, IDisposable
{
    private bool IsDisposed { get; set; }
    private static readonly IReadOnlyList<ScriptEventTypes> _eventTypes = Enum.GetValues<ScriptEventTypes>().ToList();
    private ConcurrentDictionary<short, List<EventHandler>> _events = new();

    public ScriptEventBus()
    {
        foreach (var type in _eventTypes)
            _events[(short)type] = [];
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
        IUserSessionState sessionState,
        short eventType,
        object? scriptTag = null,
        IProtocol? packet = null,
        EventArgs eventArgs = null)
    {
        if (IsDisposed) return;

        var scriptEvent = new ScriptEventParams
        {
            EventType = eventType,
            ScriptTag = scriptTag,
            EventArgs = eventArgs,
            Msg = packet,
        };

        foreach (var handler in _events[(short)eventType])
            try
            {
                handler(sessionState, scriptEvent);
            }
            catch (Exception ex)
            {
                LoggerHub.Current.Error(ex);

                if (eventType != (short)ScriptEventTypes.UnhandledError)
                    Invoke(
                        sessionState,
                        (short)ScriptEventTypes.UnhandledError,
                        sessionState.ScriptTag,
                        packet);
            }
    }

    public void RegisterEvent(
        ScriptEventTypes eventType,
        EventHandler handler)
    {
        if (IsDisposed) return;

        if (handler != null)
            _events[(short)eventType].Add(handler);
    }

    public void UnregisterEvent(
        ScriptEventTypes eventType,
        EventHandler handler)
    {
        if (IsDisposed) return;

        _events[(short)eventType].Remove(handler);
    }

    public void ClearEvents(ScriptEventTypes eventType)
    {
        if (IsDisposed) return;

        _events[(short)eventType].Clear();
    }
}