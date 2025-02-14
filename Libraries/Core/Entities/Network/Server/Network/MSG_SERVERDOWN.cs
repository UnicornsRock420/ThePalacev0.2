using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class MSG_SERVERDOWN : Core.EventParams, IStructRefNum, IProtocolS2C
    {
        [IgnoreDataMember]
        public sint32 RefNum
        {
            get => (sint32)ServerDownFlags;
            set => ServerDownFlags = (ServerDownFlags)value;
        }

        [IgnoreDataMember]
        public ServerDownFlags ServerDownFlags;

        [CString]
        public string? WhyMessage;
    }
}