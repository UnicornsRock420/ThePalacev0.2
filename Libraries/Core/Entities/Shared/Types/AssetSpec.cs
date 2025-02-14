﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using sint32 = System.Int32;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared.Types
{
    [ByteSize(8)]
    public partial class AssetSpec : IStructSerializer
    {
        public AssetSpec()
        {
            Id = 0;
            Crc = 0;
        }
        public AssetSpec(sint32 Id)
        {
            this.Id = Id;
            Crc = 0;
        }
        public AssetSpec(sint32 Id, uint32 Crc)
        {
            this.Id = Id;
            this.Crc = Crc;
        }
        public AssetSpec(Stream reader, SerializerOptions opts = SerializerOptions.None) => Deserialize(reader, opts);
        public AssetSpec(AssetSpec assetSpec)
        {
            Id = assetSpec.Id;
            Crc = assetSpec.Crc;
        }

        public sint32 Id;
        public uint32 Crc;

        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            Id = reader.ReadInt32();
            Crc = reader.ReadUInt32();
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt32(Id);
            writer.WriteUInt32(Crc);
        }
    }
}