using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("susr")]
    public partial class MSG_SUPERUSER : IProtocolC2S
    {
        [PString(1, 127)]
        public string Password;
    }
}