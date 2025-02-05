using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("pong")]
    public partial class MSG_PONG : IProtocolC2S, IProtocolS2C
    {
    }
}