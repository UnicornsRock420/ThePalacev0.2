﻿using System.Net.Sockets;
using ThePalace.Common.Factories;

namespace ThePalace.Network.Interfaces
{
    public interface IConnectionState
    {
        string? Hostname { get; set; }
        ushort Port { get; set; }

        DateTime? LastReceived { get; set; }
        DateTime? LastSent { get; set; }

        BufferStream BytesReceived { get; set; }
        byte[] Buffer { get; set; }

        Socket? Socket { get; set; }
        string? IPAddress { get; set; }

        object State { get; set; }
    }
}