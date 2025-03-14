﻿using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Logging.Entities;
using ThePalace.Scripting.Iptscrae.Enums;

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

    public void Invoke(IptEventTypes eventType, IUserSessionState<IDesktopApp> sessionState, IProtocol packet,
        object scriptState = null)
    {
        var scriptEvent = new ScriptEvent
        {
            EventType = eventType,
            Packet = packet,
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

                if (eventType != IptEventTypes.UnhandledError)
                    Invoke(IptEventTypes.UnhandledError, sessionState, packet, sessionState.ScriptTag);
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