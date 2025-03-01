using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories;
using ThePalace.Logging.Entities;
using ThePalace.Network.Interfaces;
using ConnectionState = ThePalace.Network.Entities.ConnectionState;

namespace ThePalace.Network.Factories;

public partial class AsyncTcpSocket : IDisposable
{
    public AsyncTcpSocket()
    {
        _acceptCallback = new AsyncCallback(AcceptCallback);
        _receiveCallback = new AsyncCallback(ReceiveCallback);
        _sendCallback = new AsyncCallback(SendCallback);
    }

    ~AsyncTcpSocket() => this.Dispose();

    public void Dispose()
    {
        _signalEvent.Set();

        GC.SuppressFinalize(this);
    }

    public void Shutdown()
    {
        _signalEvent.Set();

        this.Dispose();
    }

    private AsyncCallback _acceptCallback;
    private AsyncCallback _receiveCallback;
    private AsyncCallback _sendCallback;

    private volatile ManualResetEvent _signalEvent = new ManualResetEvent(false);
    public CancellationToken CancellationToken { get; private set; } = new();

    public event EventHandler ConnectionEstablished;
    public event EventHandler ConnectionReceived;
    public event EventHandler DataReceived;
    public event EventHandler StateChanged;

    public void Listen(IPEndPoint? hostAddr = null, int listenBacklog = 0)
    {
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(hostAddr));

        var ipAddress = (IPAddress?)null;

        try
        {
            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(hostAddr);
            listener.Listen(listenBacklog);

            LoggerHub.Current.Info("Listener Operational. Waiting for connections...");

            while (!CancellationToken.IsCancellationRequested)
            {
                _signalEvent.Reset();

                try
                {
                    listener.BeginAccept(_acceptCallback, listener);
                }
                catch (SocketException ex)
                {
                    LoggerHub.Current.Error(ex);
                }
                catch (Exception ex)
                {
                    LoggerHub.Current.Error(ex);
                }

                _signalEvent.WaitOne();
            }
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    public bool Connect(IConnectionState connectionState, IPEndPoint? hostAddr = null)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(connectionState));
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(hostAddr));

        DropConnection(connectionState);

        try
        {
            connectionState.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionState.Socket.Connect(hostAddr);

            connectionState.HostAddr = hostAddr;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);
        }

        try
        {
            connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);

            this.ConnectionEstablished?.Invoke(connectionState, null);

            return true;
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);
        }

        return false;
    }

    private void AcceptCallback(IAsyncResult ar)
    {
        _signalEvent.Set();

        var listener = (Socket?)ar.AsyncState;
        var handler = (Socket?)null;

        try
        {
            handler = listener?.EndAccept(ar);
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            handler = null;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            handler = null;
        }

        if (handler == null) throw new SocketException();

        var connectionState = ConnectionManager.CreateConnection(handler, ConnectionManager.Current);
        if (connectionState == null) throw new SocketException();

        handler.SetKeepAlive();

        Console.WriteLine($"{connectionState.RemoteAddr}");

        try
        {
            handler.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);

            return;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }

        ConnectionReceived.Invoke(this, connectionState);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        var connectionState = (ConnectionState?)ar.AsyncState;
        if (connectionState?.Socket == null) throw new SocketException();

        var bytesReceived = 0;

        try
        {
            bytesReceived = connectionState.Socket.EndReceive(ar);
            if (bytesReceived < 1)
            {
                DropConnection(connectionState);

                return;
            }
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);

            return;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }

        if (!IsConnected(connectionState))
        {
            DropConnection(connectionState);

            return;
        }

        using (var @lock = LockContext.GetLock(connectionState.BytesReceived))
        {
            connectionState.BytesReceived.Write(connectionState.Buffer.AsSpan(0, bytesReceived));

            connectionState.LastReceived = DateTime.UtcNow;
        }

        try
        {
            connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);

            DataReceived.Invoke(this, connectionState);
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);

            return;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    public void Send(IConnectionState connectionState, byte[]? data)
    {
        if (connectionState?.Socket == null)
        {
            DropConnection(connectionState);

            return;
        }

        if ((data?.Length ?? 0) > 0)
        {
            using (var @lock = LockContext.GetLock(connectionState.Socket))
            {
                try
                {
                    connectionState.Socket.BeginSend(data, 0, data.Length, 0, _sendCallback, connectionState);

                    connectionState.LastSent = DateTime.UtcNow;
                }
                catch (SocketException ex)
                {
                    LoggerHub.Current.Error(ex);

                    DropConnection(connectionState);

                    return;
                }
                catch (Exception ex)
                {
                    LoggerHub.Current.Error(ex);
                }
            }
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState?.Socket == null) return;

        var bytesSent = (uint)0;

        try
        {
            bytesSent = (uint)connectionState.Socket.EndSend(ar);
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);

            return;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);
        }
    }

    public static bool IsConnected(IConnectionState connectionState, int passiveIdleTimeoutInSeconds = 600)
    {
        var passiveIdleTimeout_Timespan = new TimeSpan(0, 0, passiveIdleTimeoutInSeconds);

        try
        {
            if (connectionState.LastReceived.HasValue && DateTime.UtcNow.Subtract(connectionState.LastReceived.Value) > passiveIdleTimeout_Timespan)
            {
                return (!connectionState.Socket?.Poll(1, SelectMode.SelectRead)) ?? false;
            }

            return connectionState.Socket?.Connected ?? false;
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            DropConnection(connectionState);

            return false;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            return false;
        }
    }

    public static void DropConnection(IConnectionState connectionState) =>
        connectionState?.Socket?.DropConnection();
}