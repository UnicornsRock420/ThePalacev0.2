using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using RoomID = short;

namespace Lib.Core.Entities.Network.Client.Network;

[Mnemonic("navR")]
public class MSG_ROOMGOTO : EventParams, IProtocolC2S
{
    public RoomID Dest;
}