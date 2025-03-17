using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.EventTypes.Client.Rooms
{
    [Mnemonic("opSs")]
    public class MSG_SPOTSETDESC : EventParams, IProtocol
    {
    }
}