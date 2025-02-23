using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("ryit")]
    public partial class MSG_DIYIT : EventParams, IProtocol
    {
        [IgnoreDataMember]
        public string? IpAddress;
    }
}