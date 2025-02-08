using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("pLoc")]
    public partial class MSG_PICTMOVE : IntegrationEvent, IProtocolC2S, IProtocolS2C
    {
        public sint16 RoomID;
        public sint16 SpotID;
        public Point Pos;
    }
}