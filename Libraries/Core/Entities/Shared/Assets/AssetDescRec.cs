﻿using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Interfaces.Data;
using uint16 = ushort;
using uint32 = uint;

namespace Lib.Core.Entities.Shared.Assets;

[ByteSize(40)]
public class AssetDescRec : IStruct
{
    public uint16 AssetFlags;

    [Str31] public string? Name;

    public uint16 PropFlags;
    public uint32 Size;

    public void Deserialize(Stream reader, SerializerOptions opts)
    {
        AssetFlags = reader.ReadUInt16();
        PropFlags = reader.ReadUInt16();
        Size = reader.ReadUInt32();

        Name = reader.ReadPString(1, 31, 32);
    }

    public void Serialize(Stream writer, SerializerOptions opts)
    {
        throw new NotImplementedException(nameof(AssetDescRec) + "." + nameof(Serialize));
    }
}