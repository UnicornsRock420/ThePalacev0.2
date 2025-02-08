using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("pong")]
    public partial class MSG_PONG : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
    }
}