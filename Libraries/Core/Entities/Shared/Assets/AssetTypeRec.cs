﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared.Assets
{
    [ByteSize(12)]
    public partial class AssetTypeRec : IStruct
    {
        public LegacyAssetTypes Type;
        public uint32 NbrAssets;
        public uint32 FirstAsset;
    }
}