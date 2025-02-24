﻿using System.Net.Sockets;
using ThePalace.Common.Factories;
using ThePalace.Network.Constants;
using ThePalace.Network.Interfaces;

namespace ThePalace.Network.Entities
{
    public partial class ConnectionState : EventArgs, IConnectionState
    {
        public string? Hostname { get; set; }
        public ushort Port { get; set; }

        public DateTime? LastReceived { get; set; } = null;
        public DateTime? LastSent { get; set; } = null;

        public BufferStream BytesSent { get; set; } = new();
        public BufferStream BytesReceived { get; set; } = new();
        public byte[] Buffer { get; set; } = new byte[(int)NetworkConstants.RAW_PACKET_BUFFER_SIZE];

        public Socket? Socket { get; set; } = null;
        public string? IPAddress { get; set; } = null;

        public object? State { get; set; } = null;
    }

    public partial class ConnectionState<TState> : ConnectionState
        where TState : class
    {
        public new TState? State { get; set; }
    }
}