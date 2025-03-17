using Lib.Core.Attributes.Serialization;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using uint32 = uint;

namespace Lib.Core.Entities.Shared.Assets;

[ByteSize(12)]
public class AssetTypeRec : IStruct
{
    public uint32 FirstAsset;
    public uint32 NbrAssets;
    public LegacyAssetTypes Type;
}