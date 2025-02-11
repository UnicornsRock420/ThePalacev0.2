using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wprs")]
    public partial class MSG_USERENTER : EventParams, IProtocol
    {
        public UserRec? User;
    }
}