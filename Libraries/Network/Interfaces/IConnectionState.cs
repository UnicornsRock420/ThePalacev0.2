using System.Net;
using ThePalace.Common.Entities.Network;
using ThePalace.Network.Enums;

namespace ThePalace.Network.Interfaces;

public interface IConnectionState<TSocket> : IDisposable
    where TSocket : IDisposable
{
    bool IsLittleEndian { get; set; }

    Guid Id { get; }
    CancellationTokenSource CancellationTokenSource { get; }
    CancellationToken CancellationToken { get; }

    SocketMode Mode { get; set; }

    IPEndPoint? HostAddr { get; set; }
    IPEndPoint? RemoteAddr { get; set; }

    DateTime? LastReceived { get; set; }
    DateTime? LastSent { get; set; }

    internal byte[] Buffer { get; set; }
    BufferStream? BytesReceived { get; set; }
    BufferStream? BytesSend { get; set; }

    internal TSocket? Socket { get; set; }

    //internal NetworkStream? NetworkStream { get; set; }
    object? ConnectionTag { get; set; }

    event EventHandler ConnectionEstablished;
    event EventHandler ConnectionDisconnected;
    event EventHandler ConnectionReceived;
    event EventHandler DataReceived;
    event EventHandler StateChanged;

    bool IsConnected(int passiveIdleMs = 750);

    void Connect(Uri uri);
    void Connect(IPEndPoint hostAddr);
    void Connect(IPAddress ipAddress, int port);
    void Connect(string hostname, int port);

    void Disconnect();
    
    void Shutdown();

    Task Listen(IPEndPoint hostAddr, int listenBacklog = 0);

    void Send(byte[] buffer, int offset = 0, int size = 0, bool directAccess = false);
    int Receive(byte[] buffer, int offset = 0, int size = 0);

    TSocket CreateSocket(params object[] args);
}