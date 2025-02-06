using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Shared
{
    public partial class HotspotRec
    {
        [IgnoreDataMember]
        public List<HotspotStateRec>? States;
        [IgnoreDataMember]
        public List<Point>? Vortexes;

        [IgnoreDataMember]
        [PString]
        public string? Name;
        [IgnoreDataMember]
        [CString]
        public string? Script;
    }
}