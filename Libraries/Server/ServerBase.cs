using ThePalace.Common.Server.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;

namespace ThePalace.Common.Server;

public class ServerBase
{
    public DisposableList<RoomDesc> Rooms { get; set; } = new();
    public DisposableList<IServerSessionState<IServerApp>> Users { get; set; } = new();

    public void Run()
    {
    }
}