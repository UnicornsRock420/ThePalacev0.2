using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class HotspotStateRec : IStructSerializer
    {
        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.PictID = reader.ReadInt16();
            this.Reserved = reader.ReadInt16();

            this.PicLoc = new Point(reader);
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt16(this.PictID);
            writer.WriteInt16(this.Reserved);

            this.PicLoc.Serialize(ref refNum, writer);
        }
    }
}