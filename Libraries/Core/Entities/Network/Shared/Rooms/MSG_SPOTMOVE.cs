using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;
using HotSpotID = System.Int16;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("coLs")]
    public partial class MSG_SPOTMOVE : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public RoomID RoomID;
        public HotSpotID SpotID;
        public Point Pos;
    }
}