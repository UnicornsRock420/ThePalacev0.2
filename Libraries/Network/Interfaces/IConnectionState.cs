using System.Net;
using System.Net.Sockets;
using ThePalace.Common.Factories;

namespace ThePalace.Network.Interfaces;

public interface IConnectionState
{
    IPEndPoint? HostAddr { get; set; }
    IPEndPoint? RemoteAddr { get; set; }

    DateTime? LastReceived { get; set; }
    DateTime? LastSent { get; set; }

    BufferStream BytesReceived { get; set; }
    byte[] Buffer { get; set; }

    Socket? Socket { get; set; }

    object State { get; set; }
}