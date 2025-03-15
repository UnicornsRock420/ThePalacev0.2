using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = short;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("opSd")]
public class MSG_SPOTDEL : EventParams, IProtocolC2S
{
    public HotSpotID SpotID;
}