using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("susr")]
    public partial class MSG_SUPERUSER : Core.EventParams, IProtocolC2S
    {
        [Str127]
        public string Password;
    }
}