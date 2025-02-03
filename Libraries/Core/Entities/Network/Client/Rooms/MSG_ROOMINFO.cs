using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("ofNr")]
    public partial class MSG_ROOMINFO : IProtocolC2S
    {
        public RoomRec? RoomInfo;
    }
}