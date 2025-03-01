using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using sint32 = System.Int32;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared.Assets;

[ByteSize(32)]
public partial class AssetRec : IStruct
{
    public AssetRec() : base()
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

    public AssetDescRec AssetDesc;

    public string? Md5 => Data?.ComputeMd5();

    public bool ValidateCrc() => ValidateCrc(AssetSpec?.Crc ?? 0);
    public bool ValidateCrc(uint crc) => (Data?.Length ?? 0) < 1 || crc == 0 ? false : Cipher.ComputeCrc(Data, 0, true) == crc;
}