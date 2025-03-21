using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Network;

[Mnemonic("pong")]
public class MSG_PONG : EventParams, IProtocolC2S, IProtocolS2C
{
}