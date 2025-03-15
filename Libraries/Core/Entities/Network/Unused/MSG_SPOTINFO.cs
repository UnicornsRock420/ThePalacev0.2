using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused;

[Mnemonic("ofNs")]
public partial class MSG_SPOTINFO : EventParams, IProtocol
{
    public PictureRec[] PictureList;
    public RoomID RoomID;
    public HotspotRec? SpotInfo;
}