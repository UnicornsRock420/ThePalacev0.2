using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Server.Media
{
    [Mnemonic("fnfe")]
    public partial class MSG_FILENOTFND : IProtocolS2C
    {
        public PString FileName;
    }
}