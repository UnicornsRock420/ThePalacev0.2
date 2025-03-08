﻿using ThePalace.Common.Factories.System.Collections;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Entities.Core;

public class SessionState : Disposable, ISessionState
{
    public Guid Id { get; } = Guid.NewGuid();
    public uint UserId { get; set; }
    public DateTime? LastActivity { get; set; }

    public IConnectionState? ConnectionState { get; set; } = null;
    public UserDesc? UserDesc { get; set; } = null;
    public RegistrationRec? RegInfo { get; set; } = null;

    public object? State { get; set; } = null;
}