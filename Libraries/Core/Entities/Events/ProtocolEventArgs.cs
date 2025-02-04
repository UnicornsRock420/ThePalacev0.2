using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Events
{
    public class ProtocolEventArgs : EventArgs
    {
        public int SourceID;
        public int RefNum;

        public IProtocol? Request;
        public ISessionState SessionState;

        public CancellationToken CancellationToken;
    }
}