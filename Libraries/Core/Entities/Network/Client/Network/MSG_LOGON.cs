using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("regi")]
    public partial class MSG_LOGON : Core.EventParams, IProtocolC2S
    {
        public RegistrationRec? RegInfo;
    }
}