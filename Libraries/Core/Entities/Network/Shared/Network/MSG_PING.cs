using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("ping")]
    public partial class MSG_PING : EventsBus.EventParams, IProtocolC2S, IProtocolS2C
    {
    }
}