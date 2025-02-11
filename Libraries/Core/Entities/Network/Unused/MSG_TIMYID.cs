using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.EventTypes.Client.Network
{
    [Mnemonic("timy")]
    public partial class MSG_TIMYID : EventParams, IProtocol
    {
    }
}