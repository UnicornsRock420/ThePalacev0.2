using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("nprs")]
    public partial class MSG_USERNEW : IProtocolS2C
    {
        public UserRec? User;
    }
}