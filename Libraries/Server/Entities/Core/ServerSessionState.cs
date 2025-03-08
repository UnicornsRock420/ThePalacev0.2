using ThePalace.Common.Factories.System.Collections;
using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;

namespace ThePalace.Common.Server.Entities.Core;

public class ServerSessionState : Disposable, IServerSessionState
{
    public void Dispose()
    {
        ConnectionState?.Dispose();
        ConnectionState = null;

        UserDesc = null;
        RegInfo = null;

        LastActivity = null;

        base.Dispose();
    }

    public Guid Id => Guid.NewGuid();
    public uint UserId { get; set; }

    public DateTime? LastActivity { get; set; }

    public IConnectionState? ConnectionState { get; set; }

    public UserDesc? UserDesc { get; set; } = new();
    public RegistrationRec? RegInfo { get; set; } = new();

    public object? State { get; set; }

    ~ServerSessionState()
    {
        Dispose();
    }
}