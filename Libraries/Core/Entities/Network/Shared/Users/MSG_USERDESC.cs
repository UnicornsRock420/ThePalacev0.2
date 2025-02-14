using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [DynamicSize((8 * 9) + 8, 8)]
    [Mnemonic("usrD")]
    public partial class MSG_USERDESC : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C, IStructSerializer
    {
        public sint16 FaceNbr;
        public sint16 ColorNbr;
        public sint32 NbrProps;

        [DynamicSize(8 * 9)] // AssetSpec(8) * Props(9)
        public AssetSpec[] PropSpec;

        public void Deserialize(Stream reader, SerializerOptions opts)
        {
            FaceNbr = reader.ReadInt16();
            ColorNbr = reader.ReadInt16();
            NbrProps = reader.ReadInt32();

            PropSpec = new AssetSpec[NbrProps];

            for (int j = 0; j < NbrProps; j++)
            {
                PropSpec[j] = new AssetSpec(reader, opts);
            }
        }

        public void Serialize(Stream writer, SerializerOptions opts)
        {
            writer.WriteInt16(FaceNbr);
            writer.WriteInt16(ColorNbr);

            writer.WriteInt32(PropSpec.Length);

            for (int j = 0; j < PropSpec.Length; j++)
            {
                PropSpec[j].Serialize(writer, opts);
            }
        }
    }
}