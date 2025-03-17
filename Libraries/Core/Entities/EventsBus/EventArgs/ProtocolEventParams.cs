using System.Net.Sockets;
using Lib.Core.Interfaces.Network;
using Lib.Network.Interfaces;

namespace Lib.Core.Entities.EventsBus.EventArgs;

public class ProtocolEventParams : EventParams
{
    public CancellationToken CancellationToken;
    public IConnectionState<Socket> ConnectionState;
    public int RefNum;

    public IProtocol? Request;
    public int SourceID;
}