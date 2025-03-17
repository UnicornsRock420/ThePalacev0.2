using System.Collections.Concurrent;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.Core;
using RoomID = short;
using UserID = int;

namespace Lib.Common.Server.Interfaces;

public interface IServerSessionState<TApp> : ISessionState<TApp>
    where TApp : IApp
{
    object? ScriptTag { get; set; }

    ConcurrentDictionary<string, object> Extended { get; }

    ConcurrentDictionary<RoomID, RoomDesc> Rooms { get; set; }
    ConcurrentDictionary<UserID, IUserSessionState<IApp>> Users { get; set; }
}