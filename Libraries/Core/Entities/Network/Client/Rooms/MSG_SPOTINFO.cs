using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Network;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("ofNs")]
    public partial class MSG_SPOTINFO : Core.EventParams, IProtocolC2S, IDisposable
    {
        public HotspotRec? SpotInfo;
        public PictureRec[] PictureList;
        public RoomID RoomID;

        public void Dispose()
        {
            SpotInfo = null;
        }
    }
}