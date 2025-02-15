using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("pong")]
    public partial class MSG_PONG : EventsBus.EventParams, IProtocolC2S, IProtocolS2C
    {
    }
}