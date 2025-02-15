using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrC")]
    public partial class MSG_USERCOLOR : EventParams, IProtocolC2S, IProtocolS2C
    {
        public sint16 ColorNbr;
    }
}