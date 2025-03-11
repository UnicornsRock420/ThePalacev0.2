using System.Collections.Concurrent;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Interfaces;

public interface IServerSessionState : ISessionState
{
    object? SessionTag { get; set; }
    object? ScriptTag { get; set; }
    
    ConcurrentDictionary<string, object> Extended { get; }
    
    List<RoomDesc> Rooms { get; set; }
    List<UserDesc> Users { get; set; }
}