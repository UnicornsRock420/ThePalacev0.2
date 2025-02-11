using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Rooms
{
    [Mnemonic("endr")]
    public partial class MSG_ROOMDESCEND : Core.EventParams, IProtocolS2C
    {
    }
}