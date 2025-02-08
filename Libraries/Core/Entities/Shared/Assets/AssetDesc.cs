using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Data;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared.Assets
{
    [ByteSize(40)]
    public partial class AssetDesc : IStruct
    {
        public uint16 AssetFlags;
        public uint16 PropFlags;
        public uint32 Size;

        [Str31]
        public string? Name;
    }
}