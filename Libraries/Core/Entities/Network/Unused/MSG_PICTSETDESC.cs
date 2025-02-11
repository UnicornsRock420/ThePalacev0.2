using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("sPct")]
    public partial class MSG_PICTSETDESC : EventParams, IProtocol
    {
    }
}