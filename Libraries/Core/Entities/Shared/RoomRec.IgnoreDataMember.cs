using System.Runtime.Serialization;
using sint16 = System.Int16;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomRec
    {
        [IgnoreDataMember]
        public DateTime? LastModified;
        [IgnoreDataMember]
        public sint16 MaxOccupancy;
        [IgnoreDataMember]
        public string? Name;
        [IgnoreDataMember]
        public string? Picture;
        [IgnoreDataMember]
        public string? Artist;
        [IgnoreDataMember]
        public string? Password;

        [IgnoreDataMember]
        public List<HotspotRec>? HotSpots;
        [IgnoreDataMember]
        public List<PictureRec>? Pictures;
        [IgnoreDataMember]
        public List<DrawCmdRec>? DrawCmds;
        [IgnoreDataMember]
        public List<LoosePropRec>? LooseProps;
    }
}