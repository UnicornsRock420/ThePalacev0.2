using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrD")]
    public partial class MSG_USERDESC : IProtocolC2S, IProtocolS2C
    {
        public sint16 FaceNbr;
        public sint16 ColorNbr;
        public sint32 NbrProps;

        [ByteSize(8 * 9)] // AssetSpec(8) * Props(9)
        public AssetSpec[] PropSpec;
    }
}