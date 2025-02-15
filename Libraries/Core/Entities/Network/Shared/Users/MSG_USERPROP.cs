using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrP")]
    public partial class MSG_USERPROP : EventsBus.EventParams, IProtocolC2S, IProtocolS2C
    {
        public AssetSpec[] AssetSpec;
    }
}