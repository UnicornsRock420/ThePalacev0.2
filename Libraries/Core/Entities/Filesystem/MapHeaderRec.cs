﻿using ThePalace.Core.Attributes.Serialization;
using sint32 = int;

namespace ThePalace.Core.Entities.Filesystem;

[ByteSize(24)]
public class MapHeaderRec
{
    public sint32 NbrTypes;
    public sint32 NbrAssets;
    public sint32 LenNames;
    public sint32 TypesOffset;
    public sint32 RecsOffset;
    public sint32 NamesOffset;
}