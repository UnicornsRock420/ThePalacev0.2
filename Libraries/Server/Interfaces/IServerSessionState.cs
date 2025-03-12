using System.Collections.Concurrent;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using RoomID = System.Int16;
using UserID = System.Int32;

namespace ThePalace.Common.Server.Interfaces;

public interface IServerSessionState : ISessionState
{
    object? SessionTag { get; set; }
    object? ScriptTag { get; set; }
    
    ConcurrentDictionary<string, object> Extended { get; }
    
    ConcurrentDictionary<RoomID, RoomDesc> Rooms { get; set; }
    ConcurrentDictionary<UserID, UserDesc> Users { get; set; }
}