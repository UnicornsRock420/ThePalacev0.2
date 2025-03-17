using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Network;

[Mnemonic("ping")]
public class MSG_PING : EventParams, IProtocolC2S, IProtocolS2C
{
}