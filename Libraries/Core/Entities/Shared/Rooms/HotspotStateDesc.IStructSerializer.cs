using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class HotspotStateDesc : IStructSerializer
{
    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        StateInfo.PictID = reader.ReadInt16();
        StateInfo.Reserved = reader.ReadInt16();

        StateInfo.PicLoc = new Point(reader, opts);
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteInt16(StateInfo.PictID);
        writer.WriteInt16(StateInfo.Reserved);

        StateInfo.PicLoc.Serialize(writer, opts);
    }
}