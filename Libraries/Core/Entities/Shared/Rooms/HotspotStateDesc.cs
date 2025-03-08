using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(8)]
public partial class HotspotStateDesc
{
    public HotspotStateRec StateInfo;

    public HotspotStateDesc()
    {
        StateInfo = new HotspotStateRec();
    }
}