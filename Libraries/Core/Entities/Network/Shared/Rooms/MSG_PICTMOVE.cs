using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("pLoc")]
    public partial class MSG_PICTMOVE : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public sint16 RoomID;
        public sint16 SpotID;
        public Point Pos;
    }
}