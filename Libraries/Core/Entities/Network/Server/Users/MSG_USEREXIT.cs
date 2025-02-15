using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("eprs")]
    public partial class MSG_USEREXIT : EventParams, IProtocolS2C
    {
    }
}