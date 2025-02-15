using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Rooms
{
    [Mnemonic("endr")]
    public partial class MSG_ROOMDESCEND : EventsBus.EventParams, IProtocolS2C
    {
    }
}