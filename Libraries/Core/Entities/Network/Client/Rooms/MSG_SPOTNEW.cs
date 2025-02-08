using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("opSn")]
    public partial class MSG_SPOTNEW : Core.EventParams, IProtocolC2S
    {
    }
}