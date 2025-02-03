using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Network.Enums;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(12)]
    public partial class AssetTypeRec : IProtocol
    {
        public LegacyAssetTypes Type;
        public uint32 NbrAssets;
        public uint32 FirstAsset;
    }
}