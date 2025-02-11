using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("sInf")]
    public partial class MSG_EXTENDEDINFO : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public ServerExtInfoTypes Flags;
    }
}