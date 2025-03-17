using Lib.Core.Attributes.Serialization;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using AssetID = int;
using uint32 = uint;

namespace Lib.Core.Entities.Shared.Types;

[ByteSize(8)]
public class AssetSpec : IStructSerializer
{
    public AssetID Id;
    public uint32 Crc;

    public AssetSpec()
    {
        Id = 0;
        Crc = 0;
    }

    public AssetSpec(AssetID Id)
    {
        this.Id = Id;
        Crc = 0;
    }

    public AssetSpec(AssetID Id, uint32 Crc)
    {
        this.Id = Id;
        this.Crc = Crc;
    }

    public AssetSpec(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        Deserialize(reader, opts);
    }

    public AssetSpec(AssetSpec assetSpec)
    {
        Id = assetSpec.Id;
        Crc = assetSpec.Crc;
    }

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        Id = reader.ReadInt32();
        Crc = reader.ReadUInt32();
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteInt32(Id);
        writer.WriteUInt32(Crc);
    }
}