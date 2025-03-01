using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network;

[Mnemonic("pong")]
public partial class MSG_PONG : EventParams, IProtocolC2S, IProtocolS2C
{
}