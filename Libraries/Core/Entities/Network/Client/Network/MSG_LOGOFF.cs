using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("bye ")]
    public partial class MSG_LOGOFF : Core.EventParams, IProtocolC2S
    {
    }
}