using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("ping")]
    public partial class MSG_PING : IProtocolC2S, IProtocolS2C
    {
    }
}