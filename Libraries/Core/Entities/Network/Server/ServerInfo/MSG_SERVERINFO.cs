using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [DynamicSize]
    [Mnemonic("sinf")]
    public partial class MSG_SERVERINFO : IProtocolS2C
    {
        public ServerPermissions ServerPermissions;

        [PString(1, 63)]
        public string? ServerName;

        public ServerOptions ServerOptions;
        public UploadCapabilities UlUploadCaps;
        public DownloadCapabilities UlDownloadCaps;
    }
}