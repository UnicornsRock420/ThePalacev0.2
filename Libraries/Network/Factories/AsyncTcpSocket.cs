using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ThePalace.Core.Factories;
using ThePalace.Network.Helpers;
using ConnectionState = ThePalace.Network.Entities.ConnectionState;

namespace ThePalace.Network.Factories
{
    public sealed class ServerOptions
    {
        public string HostAddress { get; set; }
        public int BindPort { get; set; }
        public int ListenBacklog { get; set; }
    }

    public partial class AsyncTcpSocket : IDisposable
    {
        ~AsyncTcpSocket() => this.Dispose();

        public void Dispose()
        {
            _signalEvent.Set();

            if (ConnectionStates != null)
            {
                using (var @lock = LockContext.GetLock(ConnectionStates))
                {
                    ConnectionStates.Values.ToList().ForEach(state =>
                    {
                        DropConnection(state);
                    });
                    ConnectionStates.Clear();
                    ConnectionStates = null;
                }
            }

            GC.SuppressFinalize(this);
        }

        public void Shutdown()
        {
            _signalEvent.Set();

            this.Dispose();
        }

        private volatile ManualResetEvent _signalEvent = new ManualResetEvent(false);
        public volatile ConcurrentDictionary<uint, ConnectionState> ConnectionStates = new();
        public CancellationToken CancellationToken { get; private set; } = new();

        public event EventHandler ConnectionReceived;
        public event EventHandler DataReceived;
        public event EventHandler StateChanged;

        public void Listen(ServerOptions opts)
        {
            if (opts == null) throw new ArgumentNullException(nameof(opts));

            var ipAddress = (IPAddress?)null;

            if (string.IsNullOrWhiteSpace(opts.HostAddress) || !IPAddress.TryParse(opts.HostAddress, out ipAddress))
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
            }

            if (ipAddress == null) throw new Exception($"Cannot bind to {opts.HostAddress}:{opts.BindPort} (address:port)!");

            try
            {
                var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                var localEndPoint = new IPEndPoint(IPAddress.Any, opts.BindPort);
                listener.Bind(localEndPoint);

                listener.Listen(opts.ListenBacklog);

#if DEBUG
                Console.WriteLine("Listener Operational. Waiting for connections...");
#endif

                while (!CancellationToken.IsCancellationRequested)
                {
                    _signalEvent.Reset();

                    try
                    {
                        listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    }
#if DEBUG
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
#else
                    catch { }
#endif

                    _signalEvent.WaitOne();
                }
            }
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            catch { }
#endif
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
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);

                handler = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                handler = null;
            }
#else
            catch
            {
                handler = null;
            }
#endif

            if (handler == null) throw new SocketException();

            var connectionState = ConnectionManager.CreateConnection(handler);
            using (var @lock = LockContext.GetLock(ConnectionStates))
            {
                var connectionId = ConnectionStates.Keys.Count > 0 ? ConnectionStates.Keys.Max() + 1 : 1;
                this.ConnectionStates.TryAdd(connectionId, connectionState);
            }

            // TODO: Check banlist record(s)

            handler.SetKeepAlive();

            Console.WriteLine($"{connectionState.IPAddress}");

            try
            {
                handler.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, new AsyncCallback(ReadCallback), connectionState);
            }
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            catch { }
#endif

            ConnectionReceived.Invoke(this, connectionState);
        }

        private void ReadCallback(IAsyncResult ar)
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
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);

                DropConnection(connectionState);

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            catch (SocketException ex)
            {
                DropConnection(connectionState);

                return;
            }
            catch { }
#endif

            if (!IsConnected(connectionState))
            {
                DropConnection(connectionState);

                return;
            }

            connectionState.BytesReceived.Write(connectionState.Buffer.AsSpan(0, bytesReceived));

            connectionState.LastReceived = DateTime.UtcNow;

            try
            {
                connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, new AsyncCallback(ReadCallback), connectionState);

                DataReceived.Invoke(this, connectionState);
            }
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);

                DropConnection(connectionState);

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            catch { }
#endif
        }

        public static void Send(ConnectionState connectionState, byte[]? data)
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
                        connectionState.Socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), connectionState);

                        connectionState.LastSent = DateTime.UtcNow;
                    }
#if DEBUG
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);

                        DropConnection(connectionState);

                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
#else
                    catch { }
#endif
                }
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            var connectionState = (ConnectionState?)ar.AsyncState;
            if (connectionState?.Socket == null) return;

            var bytesSent = (uint)0;

            try
            {
                bytesSent = (uint)connectionState.Socket.EndSend(ar);
            }
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);

                DropConnection(connectionState);

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
#else
            catch { }
#endif
        }

        public static bool IsConnected(ConnectionState connectionState, int passiveIdleTimeoutInSeconds = 600)
        {
            var passiveIdleTimeout_Timespan = new TimeSpan(0, 0, passiveIdleTimeoutInSeconds);

            try
            {
                if (connectionState.LastReceived.HasValue && DateTime.UtcNow.Subtract(connectionState.LastReceived.Value) > passiveIdleTimeout_Timespan)
                {
                    return !connectionState.Socket.Poll(1, SelectMode.SelectRead);
                }

                return connectionState.Socket.Connected;
            }
#if DEBUG
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);

                DropConnection(connectionState);

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
#else
            catch
            {
                return false;
            }
#endif
        }

        public static void DropConnection(ConnectionState connectionState) =>
            connectionState?.Socket?.DropConnection();
    }
}