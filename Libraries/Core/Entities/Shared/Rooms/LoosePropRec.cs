using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Shared.Types;
using sint16 = short;
using sint32 = int;

namespace Lib.Core.Entities.Shared.Rooms;

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