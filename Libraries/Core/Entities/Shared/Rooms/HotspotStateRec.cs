using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Data;
using sint16 = short;

namespace Lib.Core.Entities.Shared.Rooms;

[ByteSize(8)]
public class HotspotStateRec : IStruct
{
    public Point PicLoc;
    public sint16 PictID;
    public sint16 Reserved;
}