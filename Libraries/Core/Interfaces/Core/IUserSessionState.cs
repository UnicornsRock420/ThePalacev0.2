using System.Net.Sockets;
using Lib.Core.Entities.Shared.Users;
using Lib.Network.Interfaces;
using RoomID = short;
using UserID = int;

namespace Lib.Core.Interfaces.Core;

public interface IUserSessionState<TApp> : ISessionState<TApp>
    where TApp : IApp
{
    IConnectionState<Socket>? ConnectionState { get; set; }
    RoomID RoomId { get; set; }
    UserID UserId { get; set; }
    UserDesc? UserDesc { get; set; }
    RegistrationRec? RegInfo { get; set; }
    object? ScriptTag { get; set; }
}