using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.EventTypes.Client.Rooms
{
    [Mnemonic("opSs")]
    public partial class MSG_SPOTSETDESC : EventParams, IProtocol
    {
    }
}