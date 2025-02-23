using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("durl")]
    public partial class MSG_DISPLAYURL : EventParams, IProtocolS2C
    {
        [CString]
        public string? Url;
    }
}