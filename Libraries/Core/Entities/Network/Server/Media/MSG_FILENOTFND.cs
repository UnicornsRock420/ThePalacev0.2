using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Media
{
    [Mnemonic("fnfe")]
    public partial class MSG_FILENOTFND : IProtocolS2C
    {
        [PString(1, 255)]
        public string? FileName;
    }
}