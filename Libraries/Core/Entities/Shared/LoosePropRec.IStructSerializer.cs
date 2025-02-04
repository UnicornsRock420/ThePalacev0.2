using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class LoosePropRec : IStructSerializer
    {
        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.NextOfst = reader.ReadInt16();
            this.Reserved = reader.ReadInt16();

            this.AssetSpec = new AssetSpec(reader);

            this.Flags = reader.ReadInt32();
            this.RefCon = reader.ReadInt32();

            this.Loc = new Point(reader);
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            refNum = 0;

            writer.WriteInt16(this.NextOfst);
            writer.WriteInt16(this.Reserved);

            this.AssetSpec.Serialize(out refNum, writer, opts);

            writer.WriteInt32(this.Flags);
            writer.WriteInt32(this.RefCon);

            writer.WriteInt16(this.Loc.HAxis);
            writer.WriteInt16(this.Loc.VAxis);

            this.Loc.Serialize(out refNum, writer, opts);
        }
    }
}