using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("NOOP")]
    public partial class MSG_NOOP : EventParams, IProtocol
    {
    }
}