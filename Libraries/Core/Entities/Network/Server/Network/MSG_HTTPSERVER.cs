using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [DynamicSize]
    [Mnemonic("HTTP")]
    public partial class MSG_HTTPSERVER : IProtocolS2C
    {
        public CString Url;
    }
}