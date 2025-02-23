using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("rep2")]
    public partial class MSG_ALTLOGONREPLY : EventParams, IProtocolS2C
    {
        public MSG_ALTLOGONREPLY()
        {
            RegInfo = new();
        }

        public RegistrationRec? RegInfo;
    }
}