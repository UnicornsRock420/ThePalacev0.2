using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrC")]
    public partial class MSG_USERCOLOR : IProtocolC2S, IProtocolS2C
    {
        public sint16 ColorNbr;
    }
}