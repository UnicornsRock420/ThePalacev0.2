using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("vers")]
    public partial class MSG_VERSION : Core.EventParams, IProtocolS2C
    {
        public sint32 major;
        public sint32 minor;
        public sint32 revision;
        public sint32 build;
    }
}