using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [ByteSize(128)]
    [Mnemonic("regi")]
    public partial class MSG_LOGON : EventParams, IProtocolC2S
    {
        public MSG_LOGON()
        {
            RegInfo = new();
        }

        public RegistrationRec RegInfo;
    }
}