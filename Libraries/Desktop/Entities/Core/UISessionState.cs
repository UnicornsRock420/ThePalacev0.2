﻿using System.Collections.Concurrent;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;

namespace ThePalace.Common.Desktop.Entities.Core;

public class UISessionState : SessionState, IUISessionState
{
    public ConcurrentDictionary<string, object> Extended { get; set; } = new();

    public Guid Id => Guid.NewGuid();

    public uint UserId { get; set; }
    public DateTime? LastActivity { get; set; }

    public IConnectionState ConnectionState { get; set; }
    public UserDesc UserDesc { get; set; } = new();
    public RegistrationRec RegInfo { get; set; } = new();

    public object? ScriptState { get; set; }
}