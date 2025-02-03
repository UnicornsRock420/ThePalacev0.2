using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("durl")]
    public partial class MSG_DISPLAYURL : IProtocolS2C
    {
        public CString url;
    }
}