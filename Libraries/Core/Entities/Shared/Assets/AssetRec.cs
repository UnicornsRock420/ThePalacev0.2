using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using sint32 = int;
using uint16 = ushort;
using uint32 = uint;

namespace ThePalace.Core.Entities.Shared.Assets;

[ByteSize(32)]
public partial class AssetRec : IStruct
{
    public AssetDescRec AssetDesc;
    public AssetSpec AssetSpec;
    public uint16 BlockNbr;
    public sint32 BlockOffset;
    public uint32 BlockSize;
    public uint16 NbrBlocks;

    public LegacyAssetTypes Type;

    public AssetRec()
    {
        AssetSpec = new AssetSpec();
        AssetDesc = new AssetDescRec();
    }

    public string? Md5 => Data?.ComputeMd5();

    public bool ValidateCrc()
    {
        return ValidateCrc(AssetSpec?.Crc ?? 0);
    }

    public bool ValidateCrc(uint crc)
    {
        return (Data?.Length ?? 0) < 1 || crc == 0 ? false : Cipher.ComputeCrc(Data, 0, true) == crc;
    }
}