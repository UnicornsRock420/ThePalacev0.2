using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("log ")]
    public partial class MSG_USERLOG : EventsBus.EventParams, IProtocolS2C
    {
        public uint32 NbrUsers;
    }
}