using ThePalace.Core.Attributes;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Filesystem
{
    [ByteSize(24)]
    public partial struct MapHeaderRec
    {
        public sint32 NbrTypes;
        public sint32 NbrAssets;
        public sint32 LenNames;
        public sint32 TypesOffset;
        public sint32 RecsOffset;
        public sint32 NamesOffset;
    }
}