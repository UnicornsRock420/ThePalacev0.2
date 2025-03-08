using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using uint32 = uint;

namespace ThePalace.Core.Entities.Shared.Assets;

[ByteSize(12)]
public class AssetTypeRec : IStruct
{
    public uint32 FirstAsset;
    public uint32 NbrAssets;
    public LegacyAssetTypes Type;
}