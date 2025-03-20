using Lib.Common.Attributes;
using Lib.Core.Attributes.Auth;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using HotSpotID = short;

namespace Lib.Core.Entities.Network.Client.Rooms;

[Mnemonic("opSd")]
[Restricted]
public class MSG_SPOTDEL : EventParams, IProtocolC2S
{
    public HotSpotID SpotID;
}