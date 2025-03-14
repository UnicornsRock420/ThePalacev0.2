using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Entities.Network;
using ThePalace.Common.Factories.Core;
using ThePalace.Logging.Entities;
using ThePalace.Network.Constants;
using ThePalace.Network.Enums;
using ThePalace.Network.Exts.System.Net.Sockets;
using ThePalace.Network.Factories;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities;

public class ConnectionState : EventArgs, IConnectionState
{
    internal readonly AsyncCallback _acceptCallback;
    internal readonly AsyncCallback _receiveCallback;
    internal readonly AsyncCallback _sendCallback;

    public ConnectionState()
    {
        _acceptCallback = new(_AcceptCallback);
        _receiveCallback = new(_ReceiveCallback);
        _sendCallback = new(_SendCallback);
    }

    ~ConnectionState()
    {
        Dispose();
    }

    public void Dispose()
    {
        try
        {
            _acceptResetEvent?.Dispose();
        }
        finally
        {
            _acceptResetEvent = null;
        }

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

        //try
        //{
        //    NetworkStream?.Dispose();
        //}
        //finally
        //{
        //    NetworkStream = null;
        //}

        try
        {
            Socket?.DropConnection();
        }
        finally
        {
            Socket = null;
        }

        HostAddr = null;
        RemoteAddr = null;
        Buffer = null;
        ConnectionTag = null;

        LastReceived = null;
        LastSent = null;

        GC.SuppressFinalize(this);
    }

    private ManualResetEvent _acceptResetEvent = new(false);

    public Guid Id { get; } = Guid.NewGuid();
    public CancellationTokenSource CancellationTokenSource { get; } = new();
    public CancellationToken CancellationToken => CancellationTokenSource.Token;

    public SocketStatus Status { get; set; } = SocketStatus.Disconnected;

    public IPEndPoint? HostAddr { get; set; }
    public IPEndPoint? RemoteAddr { get; set; }

    public DateTime? LastReceived { get; set; }
    public DateTime? LastSent { get; set; }
    public BufferStream BytesReceived { get; set; } = new();
    public BufferStream BytesSend { get; set; } = new();
    public byte[] Buffer { get; set; } = new byte[(int)NetworkConstants.RAW_PACKET_BUFFER_SIZE];

    public Socket? Socket { get; set; }

    //public NetworkStream? NetworkStream { get; set; }
    public object? ConnectionTag { get; set; }

    public event EventHandler ConnectionEstablished;
    public event EventHandler ConnectionDisconnected;
    public event EventHandler ConnectionReceived;
    public event EventHandler DataReceived;
    public event EventHandler StateChanged;

    internal static bool Do(IConnectionState connectionState, Action cb, bool disconnectOnError = false)
    {
        try
        {
            cb();

            return true;
        }
        catch (TaskCanceledException ex)
        {
            connectionState?.Disconnect();

            return false;
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            connectionState?.Disconnect();

            return false;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            if (disconnectOnError) connectionState?.Disconnect();

            return false;
        }
    }

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
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(ConnectionState) + "." + nameof(hostAddr));

        Disconnect();

        if (!Do(
                this,
                () =>
                {
                    Socket = ConnectionManager.CreateSocket(AddressFamily.InterNetwork);
                    Socket.Connect(hostAddr);

                    Status = SocketStatus.Outbound;
                    //NetworkStream = ConnectionManager.CreateNetworkStream(Socket);
                    HostAddr = hostAddr;

                    Socket.BeginReceive(Buffer, 0, Buffer.Length, 0, _receiveCallback, this);

                    ConnectionEstablished?.Invoke(this, null);
                }))
            Disconnect();
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

    public void Disconnect()
    {
        Status = SocketStatus.Disconnected;

        Socket?.DropConnection();
        Socket = null;

        //NetworkStream?.DropConnection();
        //NetworkStream = null;

        BytesReceived?.Clear();
        BytesSend?.Clear();
    }

    public void Shutdown()
    {
        _acceptResetEvent.Set();

        Dispose();
    }

    public async Task Listen(IPEndPoint hostAddr, int listenBacklog = 0)
    {
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(ConnectionState) + "." + nameof(hostAddr));

        Do(this, () =>
        {
            Socket = ConnectionManager.CreateSocket(hostAddr.AddressFamily);
            Socket.Bind(hostAddr);
            Socket.Listen(listenBacklog);

            Status = SocketStatus.Listen;

            ConnectionManager.Current.Register(this);

            LoggerHub.Current.Debug("Listener Operational. Waiting for connections...");

            while (!CancellationToken.IsCancellationRequested)
            {
                _acceptResetEvent.Reset();

                if (!Do(this, () => Socket.BeginAccept(_acceptCallback, this))) return;

                _acceptResetEvent.WaitOne();
            }
        });
    }

    private void _AcceptCallback(IAsyncResult ar)
    {
        _acceptResetEvent.Set();

        var listenerState = (IConnectionState?)ar.AsyncState;
        if (listenerState == null ||
            listenerState != this) throw new SocketException();

        var handler = (Socket?)null;

        if (!Do(this, () => handler = listenerState.Socket.EndAccept(ar)) ||
            handler == null) throw new SocketException();

        var acceptedState = ConnectionManager.CreateConnectionState(handler, ConnectionManager.Current);

        _BeginReceive(acceptedState);

        ConnectionReceived.Invoke(acceptedState, null);
    }

    private void _BeginReceive(IConnectionState? connectionState)
    {
        connectionState ??= this;

        Do(connectionState, () => { Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState); });
    }

    private void _ReceiveCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState == null ||
            connectionState != this) return;

        if (Socket == null ||
            !IsConnected()) throw new SocketException();

        var bytesReceived = 0;

        if (!Do(connectionState, () =>
            {
                bytesReceived = connectionState.Socket.EndReceive(ar);
                if (bytesReceived < 1) throw new SocketException();
            }))
            bytesReceived = -1;

        if (bytesReceived < 1 ||
            !connectionState.IsConnected())
        {
            connectionState.Disconnect();

            return;
        }

        using (var @lock = LockContext.GetLock(connectionState.BytesReceived))
        {
            connectionState.BytesReceived.Write(connectionState.Buffer.AsSpan(0, bytesReceived));

            connectionState.LastReceived = DateTime.UtcNow;
        }

        _BeginReceive(connectionState);

        if (bytesReceived > 0)
            DataReceived.Invoke(connectionState, null);
    }

    public void Send(byte[] buffer, int offset = 0, int size = 0, bool directAccess = false)
    {
        if (Socket == null ||
            !IsConnected() ||
            (buffer?.Length ?? 0) < 1) return;

        if (size < 1)
        {
            size = buffer?.Length ?? 0;
        }

        if (directAccess)
        {
            using (var @lock = LockContext.GetLock(Socket))
            {
                Do(this, () => { Socket.BeginSend(buffer, 0, buffer.Length, 0, _sendCallback, this); });
            }
        }
        else
        {
            Do(this, () => { BytesSend?.Write(buffer, offset, size); });
        }
    }

    private void _SendCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState == null ||
            connectionState != this) return;

        if (Socket == null ||
            !IsConnected()) throw new SocketException();

        var bytesSent = (uint)0;

        Do(connectionState, () =>
        {
            bytesSent += (uint)Socket.EndSend(ar);

            if (bytesSent > 0)
                LastSent = DateTime.UtcNow;
        });
    }

    public int Receive(byte[] buffer, int offset = 0, int size = 0)
    {
        if (size < 1)
        {
            size = buffer?.Length ?? 0;
        }

        return BytesReceived.Read(buffer, offset, size);
    }
}

public class ConnectionState<TState> : ConnectionState
    where TState : class
{
    public new TState? State { get; set; }
}