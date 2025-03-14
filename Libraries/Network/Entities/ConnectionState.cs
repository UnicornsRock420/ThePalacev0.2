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
    private readonly AsyncCallback _acceptCallback;
    private readonly AsyncCallback _receiveCallback;
    private readonly AsyncCallback _sendCallback;

    private ManualResetEvent _acceptResetEvent = new(false);

    public ConnectionState()
    {
        _acceptCallback = new(AcceptCallback);
        _receiveCallback = new(ReceiveCallback);
        _sendCallback = new(SendCallback);
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

    public Guid Id { get; } = Guid.NewGuid();
    public CancellationTokenSource CancellationTokenSource { get; } = new();
    public CancellationToken CancellationToken => CancellationTokenSource.Token;

    public SocketDirection Direction { get; set; }

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

    private bool Do(Action cb, bool disconnectOnError = false)
    {
        try
        {
            cb();

            return true;
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

            if (disconnectOnError) Disconnect();

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

        if (!Do(() =>
            {
                Socket = ConnectionManager.CreateSocket(AddressFamily.InterNetwork);
                Socket.Connect(hostAddr);
                Socket.BeginReceive(Buffer, 0, Buffer.Length, 0, _receiveCallback, this);

                //NetworkStream = ConnectionManager.CreateNetworkStream(Socket);

                Direction = SocketDirection.Outbound;
                HostAddr = hostAddr;

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

    public async Task Listen(IPEndPoint hostAddr, int listenBacklog = 0)
    {
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(ConnectionState) + "." + nameof(hostAddr));

        Do(() =>
        {
            var listener = ConnectionManager.CreateSocket(hostAddr.AddressFamily);
            listener.Bind(hostAddr);
            listener.Listen(listenBacklog);

            LoggerHub.Current.Debug("Listener Operational. Waiting for connections...");

            while (!CancellationToken.IsCancellationRequested)
            {
                _acceptResetEvent.Reset();

                if (!Do(() => listener.BeginAccept(_acceptCallback, listener))) return;

                _acceptResetEvent.WaitOne();
            }
        });
    }

    private void AcceptCallback(IAsyncResult ar)
    {
        _acceptResetEvent.Set();

        var listener = (Socket?)ar.AsyncState;
        if (listener == null) throw new SocketException();

        var handler = (Socket?)null;

        if (!Do(() => handler = listener.EndAccept(ar)) ||
            handler == null) throw new SocketException();

        Socket = handler;
        //NetworkStream = ConnectionManager.CreateNetworkStream(Socket);

        Do(() => { Socket.BeginReceive(Buffer, 0, Buffer.Length, 0, _receiveCallback, this); });

        ConnectionReceived.Invoke(typeof(ConnectionState), (ConnectionState)this);
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
        if (Socket == null ||
            !IsConnected() ||
            (buffer?.Length ?? 0) < 1) return;

        if (length < 1)
        {
            length = buffer?.Length ?? 0;
        }

        if (directAccess)
        {
            Socket?.Send(buffer, length, SocketFlags.None);
        }
        else
        {
            BytesSend?.Write(buffer, offset, length);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        //var state = (IConnectionState?)ar.AsyncState;
        //if (state == null) return;

        if (Socket == null ||
            !IsConnected()) throw new SocketException();

        var bytesReceived = 0;

        if (!Do(() =>
            {
                bytesReceived = Socket.EndReceive(ar);
                if (bytesReceived < 1) bytesReceived = -1;
            }))
            bytesReceived = -1;

        if (bytesReceived < 1 ||
            !IsConnected())
        {
            Disconnect();

            return;
        }

        using (var @lock = LockContext.GetLock(BytesReceived))
        {
            BytesReceived.Write(Buffer.AsSpan(0, bytesReceived));

            LastReceived = DateTime.UtcNow;
        }

        Do(() => { Socket.BeginReceive(Buffer, 0, Buffer.Length, 0, _receiveCallback, this); });

        if (bytesReceived > 0)
            DataReceived.Invoke(typeof(ConnectionState), (ConnectionState)this);
    }

    public void Send(byte[]? data)
    {
        using (var @lock = LockContext.GetLock(Socket))
        {
            Do(() => { Socket.BeginSend(data, 0, data.Length, 0, _sendCallback, this); });
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        //var state = (IConnectionState?)ar.AsyncState;
        //if (state == null) return;

        if (Socket == null ||
            !IsConnected()) throw new SocketException();

        var bytesSent = (uint)0;

        Do(() =>
        {
            bytesSent += (uint)Socket.EndSend(ar);

            if (bytesSent > 0)
                LastSent = DateTime.UtcNow;
        });
    }

    public void Disconnect()
    {
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
}

public class ConnectionState<TState> : ConnectionState
    where TState : class
{
    public new TState? State { get; set; }
}