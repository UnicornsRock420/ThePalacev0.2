using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("mPrp")]
    public partial class MSG_PROPMOVE : EventsBus.EventParams, IProtocolC2S, IProtocolS2C
    {
        public sint32 PropNum;
        public Point Pos;
    }
}