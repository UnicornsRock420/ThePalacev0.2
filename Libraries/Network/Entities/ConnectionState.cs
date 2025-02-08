using System.Net.Sockets;
using ThePalace.Network.Constants;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities
{
    public partial class ConnectionState : EventArgs, IConnectionState
    {
        public DateTime? LastReceived { get; internal set; }
        public DateTime? LastSent { get; internal set; }
        public List<byte> BytesReceived { get; internal set; } = [];
        public byte[] Buffer { get; internal set; } = new byte[(int)NetworkConstants.RAW_PACKET_BUFFER_SIZE];
        public Socket? Socket { get; internal set; }
        public string? IPAddress { get; internal set; }
        public object? State { get; internal set; }
    }

    public partial class ConnectionState<TState> : ConnectionState
        where TState : class
    {
        public new TState? State { get; set; }
    }
}