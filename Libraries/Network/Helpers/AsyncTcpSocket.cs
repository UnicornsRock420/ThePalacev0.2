using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories.Core;
using ThePalace.Logging.Entities;
using ThePalace.Network.Entities;
using ThePalace.Network.Factories;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Helpers;

public static class AsyncTcpSocket
{
    private static readonly AsyncCallback _acceptCallback;
    private static readonly AsyncCallback _receiveCallback;
    private static readonly AsyncCallback _sendCallback;

    private static volatile ManualResetEvent _acceptResetEvent = new(false);

    static AsyncTcpSocket()
    {
        _acceptCallback = AcceptCallback;
        _receiveCallback = ReceiveCallback;
        _sendCallback = SendCallback;
    }

    public static CancellationTokenSource CancellationTokenSource { get; } = new();
    public static CancellationToken CancellationToken => CancellationTokenSource.Token;

    public static void Dispose()
    {
        _acceptResetEvent.Set();
    }

    public static void Shutdown()
    {
        _acceptResetEvent.Set();

        Dispose();
    }

    public static event EventHandler ConnectionEstablished;
    public static event EventHandler ConnectionDisconnected;
    public static event EventHandler ConnectionReceived;
    public static event EventHandler DataReceived;
    public static event EventHandler StateChanged;

    private static bool Do(this IConnectionState connectionState, Action cb, bool disconnectOnError = false)
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

    public static bool Connect(IConnectionState connectionState, IPEndPoint? hostAddr = null)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(AsyncTcpSocket) + "." + nameof(connectionState));
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(AsyncTcpSocket) + "." + nameof(hostAddr));

        connectionState.Do(() => connectionState.Disconnect());

        return connectionState.Do(() =>
        {
            connectionState.Socket = ConnectionManager.CreateSocket(AddressFamily.InterNetwork);
            connectionState.Socket.Connect(hostAddr);
            connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);

            connectionState.NetworkStream = ConnectionManager.CreateNetworkStream(connectionState.Socket);

            ConnectionEstablished?.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
        });
    }

    public static async Task Disconnect(this IConnectionState connectionState)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(AsyncTcpSocket) + "." + nameof(connectionState));

        connectionState.Disconnect();
    }

    public static async Task Listen(this IPEndPoint hostAddr, int listenBacklog = 0)
    {
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(AsyncTcpSocket) + "." + nameof(hostAddr));

        try
        {
            var listener = ConnectionManager.CreateSocket(hostAddr.AddressFamily);
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
                catch (TaskCanceledException ex)
                {
                    return;
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
        catch (TaskCanceledException ex)
        {
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
        catch (TaskCanceledException ex)
        {
            handler = null;

            return;
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

        var connectionState = ConnectionManager.CreateConnectionState(handler, ConnectionManager.Current);
        if (connectionState == null) throw new SocketException();

        connectionState.Do(() => { connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState); });

        ConnectionReceived.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState?.Socket == null) throw new SocketException();

        if (connectionState.NetworkStream.DataAvailable)
        {
            var bytesReceived = 0;

            connectionState.Do(() =>
            {
                bytesReceived = connectionState.Socket.EndReceive(ar);
                if (bytesReceived < 1) connectionState?.Disconnect();
            });

            if (!connectionState.IsConnected())
            {
                connectionState?.Disconnect();

                return;
            }

            using (var @lock = LockContext.GetLock(connectionState.BytesReceived))
            {
                connectionState.BytesReceived.Write(connectionState.Buffer.AsSpan(0, bytesReceived));

                connectionState.LastReceived = DateTime.UtcNow;
            }
        }

        connectionState.Do(() => { connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState); });

        if (connectionState.NetworkStream.DataAvailable)
            DataReceived.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
    }

    public static void Send(this IConnectionState connectionState, byte[]? data)
    {
        if (connectionState?.Socket == null ||
            (data?.Length ?? 0) < 1) return;

        using (var @lock = LockContext.GetLock(connectionState.Socket))
        {
            connectionState.Do(() => { connectionState.Socket.BeginSend(data, 0, data.Length, 0, _sendCallback, connectionState); });
        }
    }

    private static void SendCallback(IAsyncResult ar)
    {
        var connectionState = (IConnectionState?)ar.AsyncState;
        if (connectionState?.Socket == null) return;

        var bytesSent = (uint)0;

        connectionState.Do(() =>
        {
            bytesSent += (uint)connectionState.Socket.EndSend(ar);

            if (bytesSent > 0)
                connectionState.LastSent = DateTime.UtcNow;
        });
    }


    public static IPAddress Resolve(this string hostname)
    {
        return Dns.GetHostAddresses(hostname).FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);
    }
}