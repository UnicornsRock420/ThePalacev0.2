using ThePalace.Core.Interfaces.Network;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Entities.EventsBus.EventArgs
{
    public class ProtocolEventParams : EventParams
    {
        public int SourceID;
        public int RefNum;

        public IProtocol? Request;
        public IConnectionState ConnectionState;

        public CancellationToken CancellationToken;
    }
}