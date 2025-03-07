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

    private static volatile ManualResetEvent _acceptResetEvent = new(false);
    public static CancellationTokenSource CancellationTokenSource { get; } = new();
    public static CancellationToken CancellationToken => CancellationTokenSource.Token;

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
            //LoggerHub.Current.Error(ex);

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

            if (disconnectOnError)
            {
                connectionState?.Disconnect();
            }

            return false;
        }
    }

    public static async Task<bool> Connect(this IConnectionState connectionState, IPEndPoint? hostAddr = null)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(AsyncTcpSocket) + "." + nameof(connectionState));
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(AsyncTcpSocket) + "." + nameof(hostAddr));

        return connectionState.Do(() =>
        {
            ConnectionManager.Connect(connectionState, hostAddr);

            connectionState.NetworkStream.BeginRead(connectionState.Buffer, 0, connectionState.Buffer.Length, _receiveCallback, connectionState);

            ConnectionEstablished?.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
        });
    }

    public static async Task Disconnect(this IConnectionState connectionState)
    {
        ArgumentNullException.ThrowIfNull(connectionState, nameof(AsyncTcpSocket) + "." + nameof(connectionState));

        connectionState?.Disconnect();
    }

    public static async Task Listen(this IPEndPoint hostAddr, int listenBacklog = 0)
    {
        ArgumentNullException.ThrowIfNull(hostAddr, nameof(AsyncTcpSocket) + "." + nameof(hostAddr));

        try
        {
            var listener = ConnectionManager.CreateSocket(hostAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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

        connectionState.Do(() =>
        {
            connectionState.NetworkStream.BeginRead(connectionState.Buffer, 0, connectionState.Buffer.Length, _receiveCallback, connectionState);
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

            connectionState.Do(() =>
            {
                bytesReceived = connectionState.Socket.EndReceive(ar);
                if (bytesReceived < 1)
                {
                    connectionState?.Disconnect();
                }
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

        connectionState.Do(() =>
        {
            connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, _receiveCallback, connectionState);
        });

        if (connectionState.NetworkStream.DataAvailable)
        {
            DataReceived.Invoke(typeof(AsyncTcpSocket), (ConnectionState)connectionState);
        }
    }

    public static void Send(this IConnectionState connectionState, byte[]? data)
    {
        if (connectionState?.Socket == null ||
            (data?.Length ?? 0) < 1) return;

        using (var @lock = LockContext.GetLock(connectionState.Socket))
        {
            connectionState.Do(() =>
            {
                connectionState.NetworkStream.BeginWrite(data, 0, data.Length, _sendCallback, connectionState);
            });
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

    public static bool IsConnected(this IConnectionState connectionState, int passiveIdleTimeoutInSeconds = 750)
    {
        var passiveIdleTimeout_Timespan = TimeSpan.FromSeconds(passiveIdleTimeoutInSeconds);

        try
        {
            if (connectionState.LastReceived.HasValue &&
                DateTime.UtcNow.Subtract(connectionState.LastReceived.Value) > passiveIdleTimeout_Timespan)
            {
                var result = (!connectionState.Socket?.Poll(1, SelectMode.SelectRead)) ?? false;

                connectionState.LastReceived = DateTime.UtcNow;

                return result;
            }
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

            return false;
        }

        return connectionState.Socket?.Connected ?? false;
    }
}