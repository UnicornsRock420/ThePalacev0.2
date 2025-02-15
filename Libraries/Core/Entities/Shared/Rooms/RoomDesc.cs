using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared.Rooms
{
    public partial class RoomDesc : RawStream, IStruct
    {
        public RoomDesc() : base()
        {
            this.RoomInfo = new();

            this.HotSpots = new();
            this.Pictures = new();
            this.DrawCmds = new();
            this.LooseProps = new();
        }
        public RoomDesc(RoomRec room) : base()
        {
            this.RoomInfo = room;

            this.HotSpots = new();
            this.Pictures = new();
            this.DrawCmds = new();
            this.LooseProps = new();
        }
        public RoomDesc(uint8[]? data = null) : base(data)
        {
            this.HotSpots = new();
            this.Pictures = new();
            this.DrawCmds = new();
            this.LooseProps = new();
        }

        ~RoomDesc() => this.Dispose();

        public override void Dispose()
        {
            this.HotSpots?.Clear();
            this.HotSpots = null;

            this.Pictures?.Clear();
            this.Pictures = null;

            this.DrawCmds?.Clear();
            this.DrawCmds = null;

            this.LooseProps?.Clear();
            this.LooseProps = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        public RoomRec RoomInfo;

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
        [EncryptedString(1, 255)]
        public string? Password;

        [IgnoreDataMember]
        public List<HotspotDesc>? HotSpots;
        [IgnoreDataMember]
        public List<PictureRec>? Pictures;
        [IgnoreDataMember]
        public List<DrawCmdDesc>? DrawCmds;
        [IgnoreDataMember]
        public List<LoosePropRec>? LooseProps;
    }
}