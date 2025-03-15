using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;
using sint32 = int;

namespace ThePalace.Core.Entities.Network.Shared.Assets;

[Mnemonic("mPrp")]
public class MSG_PROPMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public sint32 PropNum;
}