using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;

namespace ThePalace.Core.Types
{
    [ByteSize(4)]
    public partial class Point : IStructSerializer
    {
        public Point()
        {
            this.VAxis = (sint16)RndGenerator.Next(0, 384);
            this.HAxis = (sint16)RndGenerator.Next(0, 512);
        }
        public Point(sint16 vAxis, sint16 hAxis)
        {
            this.VAxis = vAxis;
            this.HAxis = hAxis;
        }
        public Point(Stream reader)
        {
            var refNum = 0;

            this.Deserialize(ref refNum, reader, SerializerOptions.None);
        }
        public Point(Point assetSpec)
        {
            this.HAxis = assetSpec.HAxis;
            this.VAxis = assetSpec.VAxis;
        }

        public sint16 VAxis;
        public sint16 HAxis;

        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.VAxis = reader.ReadInt16();
            this.HAxis = reader.ReadInt16();
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt16(this.VAxis);
            writer.WriteInt16(this.HAxis);
        }
    }
}