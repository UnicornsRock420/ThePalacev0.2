using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("NOOP")]
    public class MSG_NOOP : EventParams, IProtocol
    {
    }
}