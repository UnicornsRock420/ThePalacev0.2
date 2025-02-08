using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("eprs")]
    public partial class MSG_USEREXIT : IntegrationEvent, IProtocolS2C
    {
    }
}