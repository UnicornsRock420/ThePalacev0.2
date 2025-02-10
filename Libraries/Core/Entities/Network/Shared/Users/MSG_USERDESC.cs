using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("usrD")]
    public partial class MSG_USERDESC : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public MSG_USERDESC()
        {
            PropSpec = new AssetSpec[9];
        }

        public sint16 FaceNbr;
        public sint16 ColorNbr;
        public sint32 NbrProps;

        [Binding(typeof(MSG_USERDESC), nameof(NbrProps))]
        [DynamicSize(8 * 9)] // AssetSpec(8) * Props(9)
        public AssetSpec[] PropSpec;
    }
}