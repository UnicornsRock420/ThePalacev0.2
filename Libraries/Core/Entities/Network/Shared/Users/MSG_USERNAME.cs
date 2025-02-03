using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrN")]
    public partial class MSG_USERNAME : IProtocolC2S, IProtocolS2C
    {
        public Str31 Name;
    }
}