using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class LoosePropRec : IStructSerializer
{
    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        NextOfst = reader.ReadInt16();
        Reserved = reader.ReadInt16();

        AssetSpec = new AssetSpec(reader);

        Flags = reader.ReadInt32();
        RefCon = reader.ReadInt32();

        Loc = new Point(reader);
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteInt16(NextOfst);
        writer.WriteInt16(Reserved);

        AssetSpec.Serialize(writer, opts);

        writer.WriteInt32(Flags);
        writer.WriteInt32(RefCon);

        writer.WriteInt16(Loc.HAxis);
        writer.WriteInt16(Loc.VAxis);

        Loc.Serialize(writer, opts);
    }
}