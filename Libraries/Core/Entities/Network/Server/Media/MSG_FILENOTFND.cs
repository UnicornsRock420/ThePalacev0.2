using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Media
{
    [Mnemonic("fnfe")]
    public partial class MSG_FILENOTFND : IntegrationEvent, IProtocolS2C
    {
        [Str255]
        public string? FileName;
    }
}