using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("rep2")]
    public partial class MSG_ALTLOGONREPLY : Core.EventParams, IProtocolS2C
    {
        public RegistrationRec? RegInfo;
    }
}