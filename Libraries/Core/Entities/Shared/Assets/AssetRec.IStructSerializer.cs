﻿using Lib.Core.Entities.Core;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;

namespace Lib.Core.Entities.Shared.Assets;

public partial class AssetRec : RawStream, IStructSerializer
{
    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        Type = (LegacyAssetTypes)reader.ReadInt32();

        AssetSpec = new AssetSpec(reader);

        BlockOffset = reader.ReadInt32();
        BlockSize = reader.ReadUInt32();
        BlockNbr = reader.ReadUInt16();
        NbrBlocks = reader.ReadUInt16();

        if (BlockNbr == 0)
        {
            AssetDesc = new AssetDescRec();
            AssetDesc.Deserialize(reader, opts);
        }

        // TODO: Data
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteInt32((int)Type);
        writer.WriteInt32(BlockOffset);
        writer.WriteUInt32(BlockSize);
        writer.WriteUInt16(BlockNbr);
        writer.WriteUInt16(NbrBlocks);

        if (BlockNbr == 0) AssetDesc.Serialize(writer, opts);

        // TODO: Data
    }
}