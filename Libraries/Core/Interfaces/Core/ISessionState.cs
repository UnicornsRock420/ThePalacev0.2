using System.Collections.Concurrent;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Interfaces.Core;

public interface ISessionState
{
    Guid Id { get; }
    DateTime? LastActivity { get; set; }
    IConnectionState? ConnectionState { get; set; }
    
    uint UserId { get; set; }
    UserDesc? UserDesc { get; set; }
    RegistrationRec? RegInfo { get; set; }
    object? State { get; set; }
    
    ConcurrentDictionary<string, object> Extended { get; }
    object? ScriptState { get; set; }

    RoomDesc RoomInfo { get; set; }
    ConcurrentDictionary<uint, UserDesc> RoomUsers { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
    int ServerPopulation { get; set; }
    List<ListRec> ServerRooms { get; set; }
    List<ListRec> ServerUsers { get; set; }
}