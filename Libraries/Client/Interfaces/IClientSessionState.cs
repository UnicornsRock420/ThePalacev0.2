using System.Collections.Concurrent;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Entities.Shared.ServerInfo;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Core;
using RoomID = short;
using UserID = int;

namespace Lib.Common.Client.Interfaces;

public interface IClientSessionState : IUserSessionState
{
    RoomDesc RoomInfo { get; set; }
    ConcurrentDictionary<UserID, UserDesc> RoomUsers { get; set; }

    int ServerPopulation { get; set; }
    ConcurrentDictionary<RoomID, ListRec> Rooms { get; set; }
    ConcurrentDictionary<UserID, ListRec> Users { get; set; }
}