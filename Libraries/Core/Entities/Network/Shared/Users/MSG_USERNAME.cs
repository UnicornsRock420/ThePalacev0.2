using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [DynamicSize(32, 1)]
    [Mnemonic("usrN")]
    public partial class MSG_USERNAME : IProtocolC2S, IProtocolS2C
    {
        [PString(1, 31)]
        public string? Name;
    }
}