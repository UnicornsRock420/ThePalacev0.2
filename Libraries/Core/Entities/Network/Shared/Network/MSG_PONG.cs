using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Network;

[Mnemonic("pong")]
public class MSG_PONG : EventParams, IProtocolC2S, IProtocolS2C
{
}