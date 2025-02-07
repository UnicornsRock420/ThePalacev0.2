using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Media
{
    [Mnemonic("qFil")]
    public partial class MSG_FILEQUERY : IProtocolC2S
    {
        [Str255]
        public string? FileName;
    }
}