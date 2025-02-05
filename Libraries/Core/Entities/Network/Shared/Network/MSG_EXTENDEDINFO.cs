using ThePalace.Core.Attributes;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("sInf")]
    public partial class MSG_EXTENDEDINFO : IProtocolC2S, IProtocolS2C
    {
        public ServerExtInfoTypes Flags;
    }
}