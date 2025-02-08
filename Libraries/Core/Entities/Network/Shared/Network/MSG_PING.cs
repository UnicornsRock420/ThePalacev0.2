using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("ping")]
    public partial class MSG_PING : IntegrationEvent, IProtocolC2S, IProtocolS2C
    {
    }
}