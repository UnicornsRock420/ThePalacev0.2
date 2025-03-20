﻿using System.Collections.Concurrent;
using Lib.Core.Entities.Scripting;
using Lib.Core.Interfaces.Core;
using Lib.Core.Interfaces.Network;
using Lib.Logging.Entities;
using Mod.Scripting.Iptscrae.Enums;

namespace ThePalace.Client.Desktop.Factories;

public class ScriptEvents : Singleton<ScriptEvents>, IDisposable
{
    private static readonly IReadOnlyList<IptEventTypes> _eventTypes = Enum.GetValues<IptEventTypes>().ToList();
    private ConcurrentDictionary<IptEventTypes, List<EventHandler>> _events = new();
    private bool IsDisposed;

    public ScriptEvents()
    {
        foreach (var type in _eventTypes)
            _events[type] = [];
    }

    ~ScriptEvents()
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
        IptEventTypes eventType,
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

                if (eventType != IptEventTypes.UnhandledError)
                    Invoke(
                        IptEventTypes.UnhandledError,
                        sessionState,
                        packet,
                        sessionState.ScriptTag);
            }
    }

    public void RegisterEvent(
        IptEventTypes eventType,
        EventHandler handler)
    {
        if (IsDisposed) return;

        if (handler != null)
            _events[eventType].Add(handler);
    }

    public void UnregisterEvent(
        IptEventTypes eventType,
        EventHandler handler)
    {
        if (IsDisposed) return;

        _events[eventType].Remove(handler);
    }

    public void ClearEvents(IptEventTypes eventType)
    {
        if (IsDisposed) return;

        _events[eventType].Clear();
    }
}