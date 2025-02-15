using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Entities.Shared.Types;

namespace ThePalace.Core.Entities.Shared.Rooms
{
    public partial class LoosePropRec : IStructSerializer
    {
        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
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

            this.AssetSpec.Serialize(writer, opts);

            writer.WriteInt32(this.Flags);
            writer.WriteInt32(this.RefCon);

            writer.WriteInt16(this.Loc.HAxis);
            writer.WriteInt16(this.Loc.VAxis);

            this.Loc.Serialize(writer, opts);
        }
    }
}