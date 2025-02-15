using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("sRom")]
    public partial class MSG_ROOMSETDESC : EventsBus.EventParams, IProtocolC2S
    {
        public RoomRec? RoomInfo;
    }
}