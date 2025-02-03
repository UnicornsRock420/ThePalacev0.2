using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.ServerInfo
{
    [ByteSize(0)]
    [Mnemonic("uLst")]
    public partial class MSG_LISTOFALLUSERS : IProtocolC2S
    {
    }
}