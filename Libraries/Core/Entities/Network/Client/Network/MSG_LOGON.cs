using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("regi")]
    public partial class MSG_LOGON : IProtocolC2S
    {
        public RegistrationRec? RegInfo;
    }
}