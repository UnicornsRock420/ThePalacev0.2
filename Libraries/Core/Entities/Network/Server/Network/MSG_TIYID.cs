using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("tiyr")]
    public partial class MSG_TIYID : IProtocolS2C
    {
        [IgnoreDataMember]
        public string? IpAddress;
    }
}