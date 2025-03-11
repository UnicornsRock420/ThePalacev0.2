using System.Collections.Concurrent;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Users;

namespace ThePalace.Core.Interfaces.Core;

public interface IClientSessionState : ISessionState
{
    uint UserId { get; set; }
    UserDesc? UserDesc { get; set; }
    RegistrationRec? RegInfo { get; set; }
    object? SessionTag { get; set; }
    object? ScriptTag { get; set; }
    
    ConcurrentDictionary<string, object> Extended { get; }

    RoomDesc RoomInfo { get; set; }
    ConcurrentDictionary<uint, UserDesc> RoomUsers { get; set; }

    int ServerPopulation { get; set; }
    List<ListRec> Rooms { get; set; }
    List<ListRec> Users { get; set; }
}