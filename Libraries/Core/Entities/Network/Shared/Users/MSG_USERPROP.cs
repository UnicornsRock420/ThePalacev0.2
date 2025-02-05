using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrP")]
    public partial class MSG_USERPROP : IProtocolC2S, IProtocolS2C
    {
        public AssetSpec[] AssetSpec;
    }
}