using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Entities.Shared.Rooms
{
    [ByteSize(8)]
    public partial class HotspotStateDesc
    {
        public HotspotStateDesc()
        {
            StateInfo = new();
        }

        public HotspotStateRec StateInfo;
    }
}