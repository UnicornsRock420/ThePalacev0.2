using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrC")]
    public partial class MSG_USERCOLOR : IntegrationEvent, IProtocolC2S, IProtocolS2C
    {
        public sint16 ColorNbr;
    }
}