using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("nRom")]
    public partial class MSG_ROOMNEW : Core.EventParams, IProtocolC2S
    {
    }
}