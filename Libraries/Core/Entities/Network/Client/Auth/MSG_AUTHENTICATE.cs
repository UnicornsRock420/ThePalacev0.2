using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("auth")]
    public partial class MSG_AUTHENTICATE : IntegrationEvent, IProtocolC2S
    {
    }
}