using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("navR")]
    public partial class MSG_ROOMGOTO : EventsBus.EventParams, IProtocolC2S
    {
        public RoomID Dest;
    }
}