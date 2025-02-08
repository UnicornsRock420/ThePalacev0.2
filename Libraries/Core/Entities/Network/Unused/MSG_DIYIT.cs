using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Entities.Shared;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("ryit")]
    [MessagePackObject(true, AllowPrivate = true)]
    public partial class MSG_DIYIT : IProtocol
    {
        [IgnoreDataMember]
        public string? IpAddress;
    }
}