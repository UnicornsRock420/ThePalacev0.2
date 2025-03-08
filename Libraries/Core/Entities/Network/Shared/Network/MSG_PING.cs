using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network;

[Mnemonic("ping")]
public class MSG_PING : EventParams, IProtocolC2S, IProtocolS2C
{
}