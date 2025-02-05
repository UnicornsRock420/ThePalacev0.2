using ThePalace.Core.Attributes;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(12)]
    public partial class AssetTypeRec : IStruct
    {
        public LegacyAssetTypes Type;
        public uint32 NbrAssets;
        public uint32 FirstAsset;
    }
}