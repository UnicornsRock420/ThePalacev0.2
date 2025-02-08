using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("susr")]
    public partial class MSG_SUPERUSER : IntegrationEvent, IProtocolC2S
    {
        [Str127]
        public string Password;
    }
}