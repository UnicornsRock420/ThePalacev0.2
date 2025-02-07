using System.Net.Sockets;
using ThePalace.Network.Entities;

namespace ThePalace.Network.Helpers
{
    internal class ConnectionManager
    {
        public static ConnectionState CreateConnection(Socket handler) =>
            new ConnectionState
            {
                Socket = handler,
            };
    }
}