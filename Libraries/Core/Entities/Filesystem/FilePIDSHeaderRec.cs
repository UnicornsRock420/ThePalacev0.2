using ThePalace.Core.Entities.Shared.Types;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Filesystem
{
    public partial struct FilePIDSHeaderRec
    {
        public AssetSpec AssetSpec;
        public sint32 dataOffset;
        public sint32 dataSize;
    }
}