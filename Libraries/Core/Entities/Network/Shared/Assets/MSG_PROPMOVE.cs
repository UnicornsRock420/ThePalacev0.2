using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("mPrp")]
    public partial class MSG_PROPMOVE : EventParams, IProtocolC2S, IProtocolS2C
    {
        public sint32 PropNum;
        public Point Pos;
    }
}