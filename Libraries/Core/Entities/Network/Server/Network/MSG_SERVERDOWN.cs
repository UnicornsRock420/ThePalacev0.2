using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class MSG_SERVERDOWN : IStructRefNum, IProtocolS2C
    {
        [RefNum]
        public ServerDownFlags ServerDownFlags;

        [CString]
        public string? WhyMessage;
    }
}