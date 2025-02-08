using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared.Assets;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Helpers;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Types;
using sint32 = System.Int32;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(32)]
    public partial class AssetRec : RawStream, IStruct
    {
        public AssetRec()
        {
            this.AssetSpec = new();
            this.AssetDesc = new();
        }

        ~AssetRec() => this.Dispose();

        public override void Dispose()
        {
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
        public AssetDesc AssetDesc;

        //public sint32 RHandle;
        //public sint32 LastUseTime;
        //public sint32 NameOffset;

        public bool ValidateCrc() => this.Length < 1 ? false : Cipher.ComputeCrc(this.Data, 0, true) == this.AssetSpec.Crc;
        public bool ValidateCrc(uint crc) => Cipher.ComputeCrc(this.Data, 0, true) == crc;
    }
}