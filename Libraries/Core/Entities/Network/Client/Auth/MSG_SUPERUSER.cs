using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("susr")]
    public partial class MSG_SUPERUSER : IProtocolC2S
    {
        public PString Password;
    }
}