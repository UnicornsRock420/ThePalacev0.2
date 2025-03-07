using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories;
using ThePalace.Logging.Entities;
using ThePalace.Network.Interfaces;
using ConnectionState = ThePalace.Network.Entities.ConnectionState;

namespace ThePalace.Network.Factories;

public static partial class AsyncTcpSocket
{
    static AsyncTcpSocket()
    {
        _acceptCallback = new AsyncCallback(AcceptCallback);
        _receiveCallback = new AsyncCallback(ReceiveCallback);
        _sendCallback = new AsyncCallback(SendCallback);
    }

    public static void Dispose()
    {
        _acceptResetEvent.Set();
    }

    public static void Shutdown()
    {
        _acceptResetEvent.Set();

        Dispose();
    }

    private static AsyncCallback _acceptCallback;
    private static AsyncCallback _receiveCallback;
    private static AsyncCallback _sendCallback;

    private static volatile ManualResetEvent _acceptResetEvent = new ManualResetEvent(false);
    public static CancellationToken CancellationToken { get; private set; } = new();

    public static event EventHandler ConnectionEstablished;
    public static event EventHandler ConnectionReceived;
    public static event EventHandler DataReceived;
    public static event EventHandler StateChanged;

    private static bool Do(IConnectionState connectionState, Action cb, bool disconnectOnError = false)
    {
        try
        {
            cb();

            return true;
        }
        catch (SocketException ex)
        {
            LoggerHub.Current.Error(ex);

            connectionState?.Socket?.DropConnection();

            return false;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            if (disconnectOnError)
                connectionState?.Socket?.DropConnection();

            return false;
        }
    }

    public static bool Connect(IConnectionState connectionState, IPEndPoint? hostAddr = null)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(connectionState));
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(hostAddr));

        connectionState?.Socket?.DropConnection();

        Do(connectionState, () =>
        {
            connectionState.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectionState.Socket.Connect(hostAddr);

            connectionState.HostAddr = hostAddr;
        });

        return Do(connectionState, () =>
        {
            connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);

            ConnectionEstablished?.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
        });
    }

    public static void Listen(IPEndPoint? hostAddr = null, int listenBacklog = 0)
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
                _acceptResetEvent.Reset();

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

                _acceptResetEvent.WaitOne();
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

    private static void AcceptCallback(IAsyncResult ar)
    {
        _acceptResetEvent.Set();

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

        Do(connectionState, () =>
        {
            handler.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);
        });

        ConnectionReceived.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState?.Socket == null) throw new SocketException();

        if (connectionState.NetworkStream.DataAvailable)
        {
            var bytesReceived = 0;

            Do(connectionState, () =>
            {
                bytesReceived = connectionState.Socket.EndReceive(ar);
                if (bytesReceived < 1)
                {
                    connectionState?.Socket?.DropConnection();
                }
            });

            if (!IsConnected(connectionState))
            {
                connectionState?.Socket?.DropConnection();

                return;
            }

            using (var @lock = LockContext.GetLock(connectionState.BytesReceived))
            {
                connectionState.BytesReceived.Write(connectionState.Buffer.AsSpan(0, bytesReceived));

                connectionState.LastReceived = DateTime.UtcNow;
            }
        }

        Do(connectionState, () =>
        {
            connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);
        });

        if (connectionState.NetworkStream.DataAvailable)
        {
            DataReceived.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
        }
    }

    public static void Send(IConnectionState connectionState, byte[]? data)
    {
        if (connectionState?.Socket == null)
        {
            connectionState?.Socket?.DropConnection();

            return;
        }

        if ((data?.Length ?? 0) > 0)
        {
            using (var @lock = LockContext.GetLock(connectionState.Socket))
            {
                Do(connectionState, () =>
                {
                    connectionState.Socket.BeginSend(data, 0, data.Length, 0, _sendCallback, connectionState);
                });
            }
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState?.Socket == null) return;

        var bytesSent = (uint)0;

        Do(connectionState, () =>
        {
            bytesSent = (uint)connectionState.Socket.EndSend(ar);

            connectionState.LastSent = DateTime.UtcNow;
        });
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

            connectionState?.Socket?.DropConnection();

            return false;
        }
        catch (Exception ex)
        {
            LoggerHub.Current.Error(ex);

            return false;
        }
    }
}