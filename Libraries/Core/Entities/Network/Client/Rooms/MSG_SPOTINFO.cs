using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Network;
using RoomID = short;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("ofNs")]
public class MSG_SPOTINFO : EventParams, IProtocolC2S, IDisposable
{
    public PictureRec[] PictureList;
    public RoomID RoomID;
    public HotspotRec? SpotInfo;

    public void Dispose()
    {
        SpotInfo = null;
    }
}