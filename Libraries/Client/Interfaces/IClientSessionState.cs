using System.Collections.Concurrent;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Network.Interfaces;
using RoomID = short;
using UserID = int;

namespace ThePalace.Common.Client.Interfaces;

public interface IClientSessionState : IUserSessionState
{
    RoomDesc RoomInfo { get; set; }
    ConcurrentDictionary<UserID, UserDesc> RoomUsers { get; set; }

    int ServerPopulation { get; set; }
    ConcurrentDictionary<RoomID, ListRec> Rooms { get; set; }
    ConcurrentDictionary<UserID, ListRec> Users { get; set; }
}