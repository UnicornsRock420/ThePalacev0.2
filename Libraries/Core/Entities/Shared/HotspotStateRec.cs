using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(8)]
    public partial class HotspotStateRec : IProtocol, IProtocolSerializer
    {
        public sint16 PictID;
        public sint16 Reserved;
        public Point PicLoc;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.PictID = reader.ReadInt16();
            this.Reserved = reader.ReadInt16();
            this.PicLoc = new Point(reader);
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt16(this.PictID);
            writer.WriteInt16(this.Reserved);

            writer.PalaceSerialize<Point>(this.PicLoc);
        }
    }
}