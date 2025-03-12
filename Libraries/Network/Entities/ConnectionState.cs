using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Entities.Network;
using ThePalace.Logging.Entities;
using ThePalace.Network.Constants;
using ThePalace.Network.Enums;
using ThePalace.Network.Exts.System.Net.Sockets;
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