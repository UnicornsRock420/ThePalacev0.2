using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrF")]
    public partial class MSG_USERFACE : EventParams, IProtocolC2S, IProtocolS2C
    {
        public sint16 FaceNbr;
    }
}