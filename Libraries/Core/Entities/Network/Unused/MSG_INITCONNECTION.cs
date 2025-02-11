using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("cLog")]
    public partial class MSG_INITCONNECTION : EventParams, IProtocol
    {
    }
}