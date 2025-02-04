using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Media
{
    [Mnemonic("qFil")]
    public partial class MSG_FILEQUERY : IProtocolC2S
    {
        [PString(1, 255)]
        public string? FileName;
    }
}