using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.App;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using sint32 = System.Int32;
using uint16 = System.UInt16;
using uint32 = System.UInt32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared.Assets
{
    [ByteSize(32)]
    public partial class AssetRec : IStruct
    {
        public AssetRec()
        {
            AssetSpec = new();
            AssetDesc = new();
        }

        public LegacyAssetTypes Type;
        public AssetSpec AssetSpec;
        public sint32 BlockOffset;
        public uint32 BlockSize;
        public uint16 BlockNbr;
        public uint16 NbrBlocks;

        [Predicate(typeof(AssetRec), nameof(BlockNbr), PredicateOperators.EqualTo, 0)]
        public AssetDescRec AssetDesc;

        [SizeDependency(typeof(AssetRec), nameof(BlockSize))]
        public uint8[] Data;

        [IgnoreDataMember]
        public string? Md5 => Data?.ComputeMd5();

        public bool ValidateCrc() => Data.Length < 1 ? false : Cipher.ComputeCrc(Data, 0, true) == AssetSpec.Crc;
        public bool ValidateCrc(uint crc) => Cipher.ComputeCrc(Data, 0, true) == crc;
    }
}