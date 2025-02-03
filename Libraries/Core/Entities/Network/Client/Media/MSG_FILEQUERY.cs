using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Client.Media
{
    [Mnemonic("qFil")]
    public partial class MSG_FILEQUERY : IProtocolC2S
    {
        public PString FileName;
    }
}