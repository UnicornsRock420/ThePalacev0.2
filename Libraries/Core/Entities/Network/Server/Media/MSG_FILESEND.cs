using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Media
{
    [Mnemonic("sFil")]
    public partial class MSG_FILESEND : IntegrationEvent, IProtocolC2S
    {
    }
}