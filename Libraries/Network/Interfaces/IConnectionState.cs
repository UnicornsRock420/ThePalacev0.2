using System.Net;
using System.Net.Sockets;

namespace ThePalace.Network.Interfaces
{
    public interface IConnectionState
    {
        DateTime? LastReceived { get; set; }
        DateTime? LastSent { get; set; }
        byte[] Buffer { get; set; }
        Socket Socket { get; set; }
        IPAddress IPAddress { get; set; }
        object State { get; set; }
    }
}