using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Entities.Network;
using ThePalace.Logging.Entities;
using ThePalace.Network.Constants;
using ThePalace.Network.Enums;
using ThePalace.Network.Exts.System.Net.Sockets;
using ThePalace.Network.Helpers;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities;

public class ConnectionState : EventArgs, IConnectionState
{
    ~ConnectionState()
    {
        Dispose();
    }

    public void Dispose()
    {
        try
        {
            BytesReceived?.Dispose();
        }
        finally
        {
            BytesReceived = null;
        }

        try
        {
            BytesSend?.Dispose();
        }
        finally
        {
            BytesSend = null;
        }

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
        ConnectionTag = null;

        LastReceived = null;
        LastSent = null;

        GC.SuppressFinalize(this);
    }

    public Guid Id { get; } = Guid.NewGuid();

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

    public object? ConnectionTag { get; set; }

    public bool IsConnected(int passiveIdleTimeoutInSeconds = 750)
    {
        var passiveIdleTimeout_Timespan = TimeSpan.FromSeconds(passiveIdleTimeoutInSeconds);

        try
        {
            if (LastReceived.HasValue &&
                DateTime.UtcNow.Subtract(LastReceived.Value) > passiveIdleTimeout_Timespan)
            {
                var result = !Socket?.Poll(1, SelectMode.SelectRead) ?? false;

                if (result)
                {
                    LastReceived = DateTime.UtcNow;
                }

                return result;
            }
        }
        catch (TaskCanceledException ex)
        {
            Disconnect();

            return false;
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            Disconnect();

            return false;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            return false;
        }

        return Socket?.Connected ?? false;
    }


    public void Connect(IPEndPoint hostAddr)
    {
        AsyncTcpSocket.Connect(this, hostAddr);

        Direction = SocketDirection.Outbound;
        HostAddr = hostAddr;
    }

    public void Connect(IPAddress ipAddress, int port)
    {
        Connect(new IPEndPoint(ipAddress, port));
    }

    public void Connect(string hostname, int port)
    {
        var ipAddr = hostname.Resolve();
        if (ipAddr != null)
        {
            Connect(ipAddr, port);
        }
    }

    public void Connect(Uri url)
    {
        Connect(url.Host, url.Port);
    }

    public int Read(byte[] buffer, int offset = 0, int length = 0)
    {
        if (length < 1)
        {
            length = buffer?.Length ?? 0;
        }
        
        return BytesReceived.Read(buffer, offset, length);
    }

    public void Write(byte[] buffer, int offset = 0, int length = 0, bool directAccess = false)
    {
        if (length < 1)
        {
            length = buffer?.Length ?? 0;
        }

        (directAccess
                ? (Stream?)NetworkStream
                : (Stream?)BytesSend)
            ?.Write(buffer, offset, length);
    }

    public void Disconnect()
    {
        Socket?.DropConnection();
        Socket = null;

        NetworkStream?.DropConnection();
        NetworkStream = null;

        BytesReceived?.Clear();
        BytesSend?.Clear();
    }
}

public class ConnectionState<TState> : ConnectionState
    where TState : class
{
    public new TState? State { get; set; }
}