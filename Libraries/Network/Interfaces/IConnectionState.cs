using System.Net.Sockets;

namespace ThePalace.Network.Interfaces
{
    public interface IConnectionState
    {
        string? Hostname { get; set; }
        ushort Port { get; set; }

        DateTime? LastReceived { get; set; }
        DateTime? LastSent { get; set; }

        MemoryStream BytesReceived { get; set; }
        byte[] Buffer { get; set; }

        Socket? Socket { get; set; }
        string? IPAddress { get; set; }

        object State { get; set; }
    }
}