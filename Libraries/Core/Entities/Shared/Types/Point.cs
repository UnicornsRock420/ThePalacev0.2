using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Shared.Types
{
    [ByteSize(4)]
    public partial class Point : IStructSerializer
    {
        public Point()
        {
            VAxis = (sint16)RndGenerator.Next(0, 384);
            HAxis = (sint16)RndGenerator.Next(0, 512);
        }
        public Point(sint16 vAxis, sint16 hAxis)
        {
            VAxis = vAxis;
            HAxis = hAxis;
        }
        public Point(Stream reader, SerializerOptions opts = SerializerOptions.None) => Deserialize(reader, opts);
        public Point(Point assetSpec)
        {
            HAxis = assetSpec.HAxis;
            VAxis = assetSpec.VAxis;
        }

        public sint16 VAxis;
        public sint16 HAxis;

        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            VAxis = reader.ReadInt16();
            HAxis = reader.ReadInt16();
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt16(VAxis);
            writer.WriteInt16(HAxis);
        }
    }
}