using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Shared.Assets;

[Mnemonic("mPrp")]
public class MSG_PROPMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public sint32 PropNum;
}