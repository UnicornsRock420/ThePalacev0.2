using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Data;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared.Assets
{
    [ByteSize(40)]
    public partial class AssetDescRec : IStruct
    {
        public uint16 AssetFlags;
        public uint16 PropFlags;
        public uint32 Size;

        [Str31]
        public string? Name;

        public void Deserialize(Stream reader, SerializerOptions opts)
        {
            AssetFlags = reader.ReadUInt16();
            PropFlags = reader.ReadUInt16();
            Size = reader.ReadUInt32();

            Name = reader.ReadPString(1, 31, 32);
        }

        public void Serialize(Stream writer, SerializerOptions opts)
        {
            throw new NotImplementedException();
        }
    }
}