using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.EventTypes.Client.Network
{
    [Mnemonic("timy")]
    public class MSG_TIMYID : EventParams, IProtocol
    {
    }
}