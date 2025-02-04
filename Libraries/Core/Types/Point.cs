using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces;
using sint16 = System.Int16;

namespace ThePalace.Core.Types
{
    [ByteSize(4)]
    public partial class Point : IStructSerializer
    {
        public Point()
        {
            this.HAxis = (sint16)RndGenerator.Next(0, 512);
            this.VAxis = (sint16)RndGenerator.Next(0, 384);
        }
        public Point(sint16 hAxis, sint16 vAxis)
        {
            this.HAxis = hAxis;
            this.VAxis = vAxis;
        }
        public Point(Stream reader)
        {
            this.Deserialize(0, reader);
        }
        public Point(Point assetSpec)
        {
            this.HAxis = assetSpec.HAxis;
            this.VAxis = assetSpec.VAxis;
        }

        public sint16 HAxis;
        public sint16 VAxis;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.HAxis = reader.ReadInt16();
            this.VAxis = reader.ReadInt16();
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            refNum = 0;

            writer.WriteInt16(this.HAxis);
            writer.WriteInt16(this.VAxis);
        }
    }
}