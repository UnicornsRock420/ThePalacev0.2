using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = System.Int16;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("opSd")]
    public partial class MSG_SPOTDEL : IProtocolC2S
    {
        public HotSpotID SpotID;
    }
}