using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Rooms
{
    [Mnemonic("endr")]
    public partial class MSG_ROOMDESCEND : IntegrationEvent, IProtocolS2C
    {
    }
}