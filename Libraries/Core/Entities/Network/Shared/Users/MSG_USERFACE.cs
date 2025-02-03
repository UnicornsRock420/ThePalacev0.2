using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrF")]
    public partial class MSG_USERFACE : IProtocolC2S, IProtocolS2C
    {
        public sint16 FaceNbr;
    }
}