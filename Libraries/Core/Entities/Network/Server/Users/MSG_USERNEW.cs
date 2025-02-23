using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("nprs")]
    public partial class MSG_USERNEW : EventParams, IProtocolS2C
    {
        public UserRec? User;
    }
}