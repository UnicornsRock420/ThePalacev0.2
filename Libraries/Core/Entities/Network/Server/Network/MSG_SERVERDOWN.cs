using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class MSG_SERVERDOWN : IProtocolRefNumOverride, IProtocolS2C
    {
        public sint32 RefNum
        {
            get => (sint32)ServerDownFlags;
            set => ServerDownFlags = (ServerDownFlags)value;
        }

        public CString WhyMessage;

        [IgnoreDataMember]
        public ServerDownFlags ServerDownFlags;
    }
}