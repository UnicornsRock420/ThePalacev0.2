﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Types;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(24)]
    public partial class LoosePropRec
    {
        public sint16 NextOfst;
        public sint16 Reserved;
        public AssetSpec AssetSpec;
        public sint32 Flags;
        public sint32 RefCon;
        public Point Loc;
    }
}