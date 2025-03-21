using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.ServerInfo;

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