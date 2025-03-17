using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Interfaces.Network;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Network;
using RoomID = short;

namespace ThePalace.Network.Entities.Unused;

[Mnemonic("ofNs")]
public class MSG_SPOTINFO : EventParams, IProtocol
{
    public PictureRec[] PictureList;
    public RoomID RoomID;
    public HotspotRec? SpotInfo;
}