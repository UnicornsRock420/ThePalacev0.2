using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("uLoc")]
    public partial class MSG_USERMOVE : IntegrationEvent, IProtocolC2S, IProtocolS2C
    {
        public Point Pos;
    }
}