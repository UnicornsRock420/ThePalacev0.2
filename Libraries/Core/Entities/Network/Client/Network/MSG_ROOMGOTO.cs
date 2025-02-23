using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("navR")]
    public partial class MSG_ROOMGOTO : EventParams, IProtocolC2S
    {
        public RoomID Dest;
    }
}