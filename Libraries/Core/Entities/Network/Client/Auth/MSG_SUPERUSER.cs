using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("susr")]
    public partial class MSG_SUPERUSER : IProtocolC2S
    {
        [Str127]
        public string Password;
    }
}