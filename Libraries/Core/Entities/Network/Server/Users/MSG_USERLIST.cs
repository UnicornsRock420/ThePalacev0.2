using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("rprs")]
    public partial class MSG_USERLIST : IProtocolS2C
    {
        public uint32 NbrUsers => (uint32)(Users?.Count ?? 0);

        public List<UserRec>? Users;
    }
}