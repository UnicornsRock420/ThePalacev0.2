using System.Net.Sockets;

namespace ThePalace.Network.Interfaces
{
    public interface IConnectionState
    {
        DateTime? LastReceived { get; }
        DateTime? LastSent { get; }
        List<byte> BytesReceived { get; }
        byte[] Buffer { get; }
        Socket? Socket { get; }
        string? IPAddress { get; }
        object State { get; }
    }
}