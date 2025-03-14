using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Entities.Network;
using ThePalace.Network.Enums;

namespace ThePalace.Network.Interfaces;

public interface IConnectionState : IDisposable
{
    Guid Id { get; }
    CancellationTokenSource CancellationTokenSource { get; }
    CancellationToken CancellationToken { get; }
    
    SocketDirection Direction { get; set; }

    IPEndPoint? HostAddr { get; set; }
    IPEndPoint? RemoteAddr { get; set; }

    DateTime? LastReceived { get; set; }
    DateTime? LastSent { get; set; }

    internal byte[] Buffer { get; set; }
    BufferStream? BytesReceived { get; set; }
    BufferStream? BytesSend { get; set; }

    internal Socket? Socket { get; set; }
    //internal NetworkStream? NetworkStream { get; set; }
    object? ConnectionTag { get; set; }

    event EventHandler ConnectionEstablished;
    event EventHandler ConnectionDisconnected;
    event EventHandler ConnectionReceived;
    event EventHandler DataReceived;
    event EventHandler StateChanged;

    bool IsConnected(int passiveIdleTimeoutInSeconds = 750);
    
    void Connect(IPEndPoint hostAddr);
    void Connect(IPAddress ipAddress, int port);
    void Connect(string hostname,  int port);
    void Connect(Uri uri);

    Task Listen(IPEndPoint hostAddr, int listenBacklog = 0);
    
    int Receive(byte[] buffer, int offset = 0, int size = 0);
    void Send(byte[] buffer, int offset = 0, int size = 0, bool directAccess = false);
    
    void Disconnect();
}