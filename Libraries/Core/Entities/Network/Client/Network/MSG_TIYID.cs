using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("tiyr")]
    public partial class MSG_TIYID : IProtocolC2S
    {
        [IgnoreDataMember]
        public string? IpAddress;
    }
}