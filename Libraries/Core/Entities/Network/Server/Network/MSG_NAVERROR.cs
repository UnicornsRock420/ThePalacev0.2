using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("sErr")]
    public partial class MSG_NAVERROR : EventParams, IProtocolS2C
    {
    }
}