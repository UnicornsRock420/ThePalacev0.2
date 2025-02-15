using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared.Rooms
{
    [ByteSize(24)]
    public partial class LoosePropRec
    {
        public sint16 NextOfst;
        public sint16 Reserved;
        public AssetSpec AssetSpec;
        public sint32 Flags;
        public sint32 RefCon;
        public Point Loc;
    }
}