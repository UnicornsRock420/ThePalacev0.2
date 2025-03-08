using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Data;
using sint16 = short;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(8)]
public class HotspotStateRec : IStruct
{
    public Point PicLoc;
    public sint16 PictID;
    public sint16 Reserved;
}