using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo;

[DynamicSize]
[Mnemonic("sinf")]
public class MSG_SERVERINFO : EventParams, IProtocolS2C
{
    [Str63] public string? ServerName;

    public ServerOptions ServerOptions;
    public ServerPermissions ServerPermissions;
    public DownloadCapabilities UlDownloadCaps;
    public UploadCapabilities UlUploadCaps;
}