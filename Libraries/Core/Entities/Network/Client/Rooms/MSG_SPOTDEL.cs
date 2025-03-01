using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("opSd")]
public partial class MSG_SPOTDEL : EventParams, IProtocolC2S
{
    public HotSpotID SpotID;
}