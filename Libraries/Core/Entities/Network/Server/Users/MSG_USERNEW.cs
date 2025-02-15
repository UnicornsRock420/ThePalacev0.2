using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("nprs")]
    public partial class MSG_USERNEW : EventsBus.EventParams, IProtocolS2C
    {
        public UserRec? User;
    }
}