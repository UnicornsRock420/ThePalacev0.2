using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("mPrp")]
    public partial class MSG_PROPMOVE : IProtocolC2S, IProtocolS2C
    {
        public sint32 PropNum;
        public Point Pos;
    }
}