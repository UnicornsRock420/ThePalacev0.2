using Lib.Common.Server.Interfaces;
using Lib.Core.Entities.Shared.Rooms;

namespace Lib.Common.Server;

public class ServerBase
{
    public DisposableList<RoomDesc> Rooms { get; set; } = new();
    public DisposableList<IServerSessionState<IServerApp>> Users { get; set; } = new();

    public void Run()
    {
    }
}