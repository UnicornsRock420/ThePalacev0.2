using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("navR")]
    public partial class MSG_ROOMGOTO : IProtocolC2S
    {
        public RoomID Dest;
    }
}