using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("uLoc")]
    public partial class MSG_USERMOVE : IProtocolC2S, IProtocolS2C
    {
        public Point Pos;
    }
}