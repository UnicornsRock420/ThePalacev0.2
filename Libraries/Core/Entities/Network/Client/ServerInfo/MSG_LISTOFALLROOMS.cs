using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.ServerInfo
{
    [Mnemonic("rLst")]
    public partial class MSG_LISTOFALLROOMS : IntegrationEvent, IProtocolC2S
    {
    }
}