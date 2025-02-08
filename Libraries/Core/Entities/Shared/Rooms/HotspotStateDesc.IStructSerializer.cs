using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class HotspotStateDesc : IStructSerializer
    {
        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            StateInfo.PictID = reader.ReadInt16();
            StateInfo.Reserved = reader.ReadInt16();

            StateInfo.PicLoc = new Point(reader);
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt16(StateInfo.PictID);
            writer.WriteInt16(StateInfo.Reserved);

            StateInfo.PicLoc.Serialize(ref refNum, writer);
        }
    }
}