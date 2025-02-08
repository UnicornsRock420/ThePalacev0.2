using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [DynamicSize(32, 1)]
    [Mnemonic("usrN")]
    public partial class MSG_USERNAME : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        [Str31]
        public string? Name;
    }
}