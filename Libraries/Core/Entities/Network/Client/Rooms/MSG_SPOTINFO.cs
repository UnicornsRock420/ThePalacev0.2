using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("ofNs")]
    public partial class MSG_SPOTINFO : IProtocolC2S, IDisposable
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