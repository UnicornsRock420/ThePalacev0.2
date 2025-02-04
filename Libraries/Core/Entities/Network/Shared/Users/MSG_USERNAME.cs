using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrN")]
    public partial class MSG_USERNAME : IProtocolC2S, IProtocolS2C
    {
        [PString(1, 31)]
        public string? Name;
    }
}