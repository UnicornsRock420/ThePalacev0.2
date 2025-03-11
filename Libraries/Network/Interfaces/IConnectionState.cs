using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Entities.Network;
using ThePalace.Network.Enums;

namespace ThePalace.Network.Interfaces;

public interface IConnectionState : IDisposable
{
    Guid Id { get; }
    
    SocketDirection Direction { get; set; }

    IPEndPoint? HostAddr { get; set; }
    IPEndPoint? RemoteAddr { get; set; }

    DateTime? LastReceived { get; set; }
    DateTime? LastSent { get; set; }

    byte[] Buffer { get; set; }
    BufferStream? BytesReceived { get; set; }
    BufferStream? BytesSend { get; set; }

    internal Socket? Socket { get; set; }
    internal NetworkStream? NetworkStream { get; set; }

    object? ConnectionTag { get; set; }

    bool IsConnected(int passiveIdleTimeoutInSeconds = 750);
    
    void Disconnect();
}