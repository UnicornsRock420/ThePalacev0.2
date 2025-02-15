using ThePalace.Core.Interfaces.Network;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Entities.EventParams
{
    public class ProtocolEventParams : Core.EventParams
    {
        public int SourceID;
        public int RefNum;

        public IProtocol? Request;
        public IConnectionState ConnectionState;

        public CancellationToken CancellationToken;
    }
}