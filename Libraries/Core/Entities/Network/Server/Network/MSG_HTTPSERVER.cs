using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [DynamicSize]
    [Mnemonic("HTTP")]
    public partial class MSG_HTTPSERVER : IProtocolS2C
    {
        [CString]
        public string? Url;
    }
}