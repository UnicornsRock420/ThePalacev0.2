using System.Collections.Concurrent;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using RoomID = System.Int16;
using UserID = System.Int32;

namespace ThePalace.Common.Client.Interfaces;

public interface IClientSessionState : ISessionState
{
    UserID UserId { get; set; }
    UserDesc? UserDesc { get; set; }
    RegistrationRec? RegInfo { get; set; }
    object? SessionTag { get; set; }
    object? ScriptTag { get; set; }
    
    ConcurrentDictionary<string, object> Extended { get; }

    RoomDesc RoomInfo { get; set; }
    ConcurrentDictionary<UserID, UserDesc> RoomUsers { get; set; }

    int ServerPopulation { get; set; }
    ConcurrentDictionary<RoomID, ListRec> Rooms { get; set; }
    ConcurrentDictionary<UserID, ListRec> Users { get; set; }
}