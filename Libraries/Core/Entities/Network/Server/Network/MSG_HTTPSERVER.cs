using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [DynamicSize]
    [Mnemonic("HTTP")]
    public partial class MSG_HTTPSERVER : Core.EventParams, IProtocolS2C
    {
        [CString]
        public string? Url;
    }
}