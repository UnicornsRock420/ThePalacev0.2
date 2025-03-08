using ThePalace.Core.Interfaces.Network;
using ThePalace.Network.Interfaces;

namespace ThePalace.Core.Entities.EventsBus.EventArgs;

public class ProtocolEventParams : EventParams
{
    public CancellationToken CancellationToken;
    public IConnectionState ConnectionState;
    public int RefNum;

    public IProtocol? Request;
    public int SourceID;
}