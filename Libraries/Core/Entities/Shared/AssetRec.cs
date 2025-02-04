using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Core;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using ThePalace.Network.Enums;
using sint32 = System.Int32;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(32)]
    public partial class AssetRec : RawData, IStruct
    {
        public override void Dispose()
        {
            _data?.Clear();
            _data = null;

            //try { Image?.Dispose(); Image = null; } catch { }

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        public LegacyAssetTypes Type;
        public AssetSpec AssetSpec;
        public sint32 BlockOffset;
        public uint32 BlockSize;
        public uint16 BlockNbr;
        public uint16 NbrBlocks;
        public AssetDescriptorRec Desc;

        //public sint32 RHandle;
        //public sint32 LastUseTime;
        //public sint32 NameOffset;

        public bool ValidateCrc() => (Data?.Length ?? 0) < 1 ? false : Cipher.ComputeCrc(Data, 0, true) == AssetSpec.Crc;
        public bool ValidateCrc(uint crc) => Cipher.ComputeCrc(Data, 0, true) == crc;
    }
}