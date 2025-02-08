using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("durl")]
    public partial class MSG_DISPLAYURL : IProtocolS2C
    {
        [CString]
        public string? Url;
    }
}