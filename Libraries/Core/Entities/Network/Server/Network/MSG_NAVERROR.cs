using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("sErr")]
    public partial class MSG_NAVERROR : EventsBus.EventParams, IProtocolS2C
    {
    }
}