using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Network;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wprs")]
    public class MSG_USERENTER : EventParams, IProtocol
    {
        public UserRec? User;
    }
}