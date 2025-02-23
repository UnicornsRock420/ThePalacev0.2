using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("resp")]
    public partial class MSG_RESPORT : EventParams, IProtocol
    {
    }
}