using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Media
{
    [Mnemonic("qFil")]
    public partial class MSG_FILEQUERY : EventsBus.EventParams, IProtocolC2S
    {
        [Str255]
        public string? FileName;
    }
}