using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Client.Assets
{
    [Mnemonic("dPrp")]
    public partial class MSG_PROPDEL : EventParams, IProtocolC2S, IProtocolS2C
    {
        public sint32 PropNum;
    }
}