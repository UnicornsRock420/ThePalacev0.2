using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("bye ")]
    public partial class MSG_LOGOFF : IProtocolS2C
    {
        public sint32 NbrUsers;
    }
}