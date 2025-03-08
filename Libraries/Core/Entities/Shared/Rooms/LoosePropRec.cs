using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using sint16 = short;
using sint32 = int;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(24)]
public partial class LoosePropRec
{
    public AssetSpec AssetSpec;
    public sint32 Flags;
    public Point Loc;
    public sint16 NextOfst;
    public sint32 RefCon;
    public sint16 Reserved;
}