using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(24)]
    public partial class LoosePropRec : IProtocol, IProtocolSerializer
    {
        public sint16 NextOfst;
        public sint16 Reserved;
        public AssetSpec AssetSpec;
        public sint32 Flags;
        public sint32 RefCon;
        public Point Loc;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.NextOfst = reader.ReadInt16();
            this.Reserved = reader.ReadInt16();
            this.AssetSpec = new AssetSpec(reader);
            this.Flags = reader.ReadInt32();
            this.RefCon = reader.ReadInt32();
            this.Loc = new Point(reader);
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt16(this.NextOfst);
            writer.WriteInt16(this.Reserved);

            writer.WriteInt32(this.AssetSpec.Id);
            writer.WriteUInt32(this.AssetSpec.Crc);

            writer.WriteInt32(this.Flags);
            writer.WriteInt32(this.RefCon);

            writer.WriteInt16(this.Loc.HAxis);
            writer.WriteInt16(this.Loc.VAxis);
        }
    }
}