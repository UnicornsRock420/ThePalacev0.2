using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo;

[DynamicSize]
[Mnemonic("sinf")]
public partial class MSG_SERVERINFO : EventParams, IProtocolS2C
{
    public ServerPermissions ServerPermissions;

    [Str63]
    public string? ServerName;

    public ServerOptions ServerOptions;
    public UploadCapabilities UlUploadCaps;
    public DownloadCapabilities UlDownloadCaps;
}