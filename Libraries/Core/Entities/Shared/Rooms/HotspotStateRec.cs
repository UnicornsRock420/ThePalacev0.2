using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(8)]
public partial class HotspotStateRec : IStruct
{
    public sint16 PictID;
    public sint16 Reserved;
    public Point PicLoc;
}