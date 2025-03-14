﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums;
using ThePalace.Core.Helpers.Core;
using ThePalace.Core.Interfaces.Data;
using sint32 = int;
using uint16 = ushort;
using uint32 = uint;

namespace ThePalace.Core.Entities.Shared.Assets;

[ByteSize(32)]
public partial class AssetRec : IStruct
{
    public LegacyAssetTypes Type;

    public AssetDescRec AssetDesc = new();
    public AssetSpec AssetSpec = new();

    public uint16 BlockNbr;
    public sint32 BlockOffset;
    public uint32 BlockSize;
    public uint16 NbrBlocks;

    public string? Md5 => Data?.ComputeMd5();

    public bool ValidateCrc()
    {
        return ValidateCrc(AssetSpec?.Crc ?? 0);
    }

    public bool ValidateCrc(uint crc)
    {
        return (Data?.Length ?? 0) < 1 ||
               crc == 0
            ? false
            : Cipher.ComputeCrc(Data, 0, true) == crc;
    }
}