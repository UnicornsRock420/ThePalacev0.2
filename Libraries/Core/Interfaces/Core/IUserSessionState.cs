using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;
using RoomID = short;
using UserID = int;

namespace ThePalace.Core.Interfaces.Core;

public interface IUserSessionState<TApp> : ISessionState<TApp>
    where TApp : IApp
{
    IConnectionState? ConnectionState { get; set; }
    RoomID RoomId { get; set; }
    UserID UserId { get; set; }
    UserDesc? UserDesc { get; set; }
    RegistrationRec? RegInfo { get; set; }
    object? ScriptTag { get; set; }
}