using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Entities.Network;
using ThePalace.Network.Constants;
using ThePalace.Network.Enums;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities;

public class ConnectionState : EventArgs, IConnectionState
{
    public BufferStream BytesSent { get; set; } = new();

    public void Dispose()
    {
        try
        {
            BytesSent?.Dispose();
        }
        catch
        {
        }

        BytesSent = null;
        try
        {
            BytesReceived?.Dispose();
        }
        catch
        {
        }

        BytesReceived = null;

        try
        {
            NetworkStream?.Dispose();
        }
        catch
        {
        }

        NetworkStream = null;
        try
        {
            Socket?.DropConnection();
        }
        catch
        {
        }

        try
        {
            Socket?.Dispose();
        }
        catch
        {
        }

        Socket = null;

        HostAddr = null;
        RemoteAddr = null;
        Buffer = null;
        State = null;

        LastReceived = null;
        LastSent = null;

        GC.SuppressFinalize(this);
    }

    public SocketDirection Direction { get; set; }

    public IPEndPoint? HostAddr { get; set; }
    public IPEndPoint? RemoteAddr { get; set; }

    public DateTime? LastReceived { get; set; }
    public DateTime? LastSent { get; set; }
    public BufferStream BytesReceived { get; set; } = new();
    public BufferStream BytesSend { get; set; } = new();
    public byte[] Buffer { get; set; } = new byte[(int)NetworkConstants.RAW_PACKET_BUFFER_SIZE];

    public Socket? Socket { get; set; }
    public NetworkStream? NetworkStream { get; set; }

    public object? State { get; set; }

    ~ConnectionState()
    {
        Dispose();
    }
}

public class ConnectionState<TState> : ConnectionState
    where TState : class
{
    public new TState? State { get; set; }
}