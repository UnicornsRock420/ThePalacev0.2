using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;

namespace Lib.Core.Entities.Shared.Rooms;

public partial class HotspotStateDesc : IStructSerializer
{
    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        PictID = reader.ReadInt16();
        Reserved = reader.ReadInt16();

        PicLoc = new Point(reader, opts);
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteInt16(PictID);
        writer.WriteInt16(Reserved);

        PicLoc.Serialize(writer, opts);
    }
}