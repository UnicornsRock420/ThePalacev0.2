using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using ThePalace.Network.Entities;
using ThePalace.Network.Helpers;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Factories
{
    public partial class AsyncTcpSocket : IDisposable
    {
        ~AsyncTcpSocket() => this.Dispose();

        public void Dispose()
        {
            _signalEvent.Set();

            if (ConnectionStates != null)
            {
                lock (ConnectionStates)
                {
                    ConnectionStates.Values.ToList().ForEach(state =>
                    {
                        state.Socket.Disconnect(false);
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

        public void Listen(string bindAddress, short bindPort, int listenBacklog = 0)
        {
            var ipAddress = (IPAddress?)null;

            if (string.IsNullOrWhiteSpace(bindAddress) || !IPAddress.TryParse(bindAddress, out ipAddress))
            {
                var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                ipAddress = ipHostInfo.AddressList[0];
            }

            if (ipAddress == null) throw new Exception($"Cannot bind to {bindAddress}:{bindPort} (address:port)!");

            var localEndPoint = new IPEndPoint(IPAddress.Any, bindPort);
            var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);

                Console.WriteLine("Listener Operational. Waiting for connections...");

                listener.Listen(listenBacklog);

                while (!CancellationToken.IsCancellationRequested)
                {
                    _signalEvent.Reset();

                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    _signalEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
            }
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
            catch (Exception ex)
            {
                handler = null;
            }

            if (handler == null) throw new SocketException();

            var connectionState = ConnectionManager.CreateConnection(handler);
            //connectionState.IPAddress = handler.GetIPAddress();

            // TODO: Check banlist record(s)

            //handler.SetKeepAlive();

            // Logger.Info: Connection from: {connectionState.IPAddress}[{sessionState.UserID}]

            try
            {
                handler.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, new AsyncCallback(ReadCallback), connectionState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ConnectionReceived.Invoke(this, connectionState);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var connectionState = (ConnectionState?)ar.AsyncState;
            if (connectionState?.Socket == null) throw new SocketException();

            var bytesReceived = (uint)0;

            try
            {
                bytesReceived = (uint)connectionState.Socket.EndReceive(ar);
                if (bytesReceived < 1)
                {
                    connectionState.Socket.Disconnect(false);

                    return;
                }
            }
            catch (SocketException ex)
            {
                connectionState.Socket.Disconnect(false);

                return;
            }
            catch (Exception ex)
            {
                connectionState.Socket.Disconnect(false);

                return;
            }

            connectionState.LastReceived = DateTime.UtcNow;

            try
            {
                connectionState.Socket.BeginReceive(connectionState.Buffer, 0, connectionState.Buffer.Length, 0, new AsyncCallback(ReadCallback), connectionState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DataReceived.Invoke(this, connectionState);
        }

        public void Send(ConnectionState connectionState, byte[] data)
        {
            if (connectionState?.Socket == null) return;

            if (data.Length > 0)
            {
                connectionState.LastSent = DateTime.UtcNow;

                try
                {
                    lock (connectionState.Socket)
                    {
                        connectionState.Socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), connectionState);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            var connectionState = (ConnectionState?)ar.AsyncState;
            if (connectionState?.Socket == null) return;

            var bytesSent = (uint)0;

            try
            {
                bytesSent = (uint)connectionState.Socket.EndSend(ar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool IsConnected(ConnectionState connectionState)
        {
            var passiveIdleTimeout_Timespan = new TimeSpan(0, 0, 600);

            try
            {
                if (connectionState.LastReceived.HasValue && DateTime.UtcNow.Subtract(connectionState.LastReceived.Value) > passiveIdleTimeout_Timespan)
                {
                    return !connectionState.Socket.Poll(1, SelectMode.SelectRead);
                }

                return connectionState.Socket.Connected;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void DropConnection(ConnectionState connectionState)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }
    }
}