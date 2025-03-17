using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Shared.Assets;

[Mnemonic("dPrp")]
public class MSG_PROPDEL : EventParams, IProtocolC2S, IProtocolS2C
{
    public sint32 PropNum;
}