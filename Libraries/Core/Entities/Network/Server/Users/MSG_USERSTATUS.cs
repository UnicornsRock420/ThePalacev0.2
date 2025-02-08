using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("uSta")]
    public partial class MSG_USERSTATUS : IntegrationEvent, IProtocolS2C
    {
        public UserFlags Flags;
        [IgnoreDataMember]
        public Guid Hash;
    }
}