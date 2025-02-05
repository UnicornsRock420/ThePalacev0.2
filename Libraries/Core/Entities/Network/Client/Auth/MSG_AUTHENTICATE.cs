using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth
{
    [Mnemonic("auth")]
    public partial class MSG_AUTHENTICATE : IProtocolC2S
    {
    }
}