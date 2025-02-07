using System.Net;
using System.Net.Sockets;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities
{
    public partial class ConnectionState : EventArgs, IConnectionState
    {
        public DateTime? LastReceived { get; set; }
        public DateTime? LastSent { get; set; }
        public byte[] Buffer { get; set; } = new byte[4096];
        public Socket Socket { get; set; }
        public IPAddress IPAddress { get; set; }
        public object? State { get; set; }
    }

    public partial class ConnectionState<TState> : ConnectionState
        where TState : class
    {
        public new TState? State { get; set; }
    }
}