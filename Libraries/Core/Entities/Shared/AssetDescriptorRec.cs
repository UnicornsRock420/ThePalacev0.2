﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Data;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(40)]
    public partial class AssetDescriptorRec : IStruct
    {
        public uint16 AssetFlags;
        public uint16 PropFlags;
        public uint32 Size;

        [PString(1, 31)]
        public string? Name;
    }
}