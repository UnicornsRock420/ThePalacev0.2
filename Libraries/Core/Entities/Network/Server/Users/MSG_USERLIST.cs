using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.Users
{
    [Mnemonic("rprs")]
    public partial class MSG_USERLIST : IProtocolRefNumOverride, IProtocolS2C
    {
        [RefNum]
        public sint32 RefNum
        {
            get => this.Users?.Count ?? 0;
            set
            {
                if (value > 0) return;

                this.Users = [];
            }
        }

        public List<UserRec>? Users;
    }
}