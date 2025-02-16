using System.Net.Sockets;
using ThePalace.Network.Constants;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities
{
    public partial class ConnectionState : EventArgs, IConnectionState
    {
        internal object LockObject_BytesReceived { get; set; } = new();
        internal object LockObject_Socket { get; set; } = new();

        public DateTime? LastReceived { get; internal set; } = null;
        public DateTime? LastSent { get; internal set; } = null;
        public MemoryStream BytesReceived { get; internal set; } = new();
        public byte[] Buffer { get; internal set; } = new byte[(int)NetworkConstants.RAW_PACKET_BUFFER_SIZE];
        public Socket? Socket { get; internal set; } = null;
        public string? IPAddress { get; internal set; } = null;
        public object? State { get; internal set; } = null;
    }

    public partial class ConnectionState<TState> : ConnectionState
        where TState : class
    {
        public new TState? State { get; set; }
    }
}