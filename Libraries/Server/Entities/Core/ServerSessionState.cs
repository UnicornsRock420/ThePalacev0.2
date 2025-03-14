﻿using System.Collections;
using System.Collections.Concurrent;
using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;
using RoomID = short;
using UserID = int;

namespace ThePalace.Common.Server.Entities.Core;

public class ServerSessionState : Disposable, IServerSessionState<IServerApp>
{
    ~ServerSessionState()
    {
        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public IServerApp App { get; set; }
    public Guid Id => Guid.NewGuid();

    public object? SessionTag { get; set; } = null;
    public object? ScriptTag { get; set; } = null;

    public ConcurrentDictionary<string, object> Extended { get; }

    public string? MediaUrl { get; set; } = null;
    public string? ServerName { get; set; } = null;
    public ConcurrentDictionary<RoomID, RoomDesc> Rooms { get; set; } = new();
    public ConcurrentDictionary<UserID, IUserSessionState<IApp>> Users { get; set; } = new();
}