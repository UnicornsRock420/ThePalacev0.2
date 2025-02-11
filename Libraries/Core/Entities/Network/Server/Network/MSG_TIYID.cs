using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("tiyr")]
    public partial class MSG_TIYID : Core.EventParams, IProtocolS2C
    {
        [IgnoreDataMember]
        public string? IpAddress;
    }
}