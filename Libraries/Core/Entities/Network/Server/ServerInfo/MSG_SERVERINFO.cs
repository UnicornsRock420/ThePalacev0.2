using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint32 = System.Int32;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [Mnemonic("sinf")]
    public partial class MSG_SERVERINFO : IProtocolS2C
    {
        public sint32 ServerPermissions;
        public Str63 ServerName;
        public uint32 serverOptions;
        public uint32 ulUploadCaps;
        public uint32 ulDownloadCaps;
    }
}