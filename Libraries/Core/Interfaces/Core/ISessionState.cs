using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Interfaces.Core;

public interface ISessionState
{
    Guid Id { get; }
    uint UserId { get; set; }
    DateTime? LastActivity { get; set; }

    IConnectionState? ConnectionState { get; set; }
    UserDesc? UserDesc { get; set; }
    RegistrationRec? RegInfo { get; set; }

    object? State { get; set; }
}