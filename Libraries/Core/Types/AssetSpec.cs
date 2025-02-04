using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;
using uint32 = System.UInt32;

namespace ThePalace.Core.Types
{
    [ByteSize(8)]
    public partial class AssetSpec : IProtocolSerializer
    {
        public AssetSpec()
        {
            this.Id = 0;
            this.Crc = 0;
        }
        public AssetSpec(sint32 Id)
        {
            this.Id = Id;
            this.Crc = 0;
        }
        public AssetSpec(sint32 Id, uint32 Crc)
        {
            this.Id = Id;
            this.Crc = Crc;
        }
        public AssetSpec(Stream reader)
        {
            this.Deserialize(0, reader);
        }
        public AssetSpec(AssetSpec assetSpec)
        {
            this.Id = assetSpec.Id;
            this.Crc = assetSpec.Crc;
        }

        public sint32 Id;
        public uint32 Crc;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.Id = reader.ReadInt32();
            this.Crc = reader.ReadUInt32();
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            refNum = 0;

            writer.WriteInt32(this.Id);
            writer.WriteUInt32(this.Crc);
        }
    }
}