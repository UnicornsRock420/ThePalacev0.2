using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Core;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using RoomID = System.Int16;
using sint16 = System.Int16;
using sint32 = System.Int32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomRec : RawData, IDisposable, IData, IProtocolSerializer
    {
        public RoomRec()
        {
            _data = new();

            HotSpots = new();
            Pictures = new();
            DrawCmds = new();
            LooseProps = new();
        }
        public RoomRec(uint8[]? data = null)
        {
            _data = new(data);

            HotSpots = new();
            Pictures = new();
            DrawCmds = new();
            LooseProps = new();
        }

        public override void Dispose()
        {
            _data?.Clear();
            _data = null;

            HotSpots?.Clear();
            HotSpots = null;

            Pictures?.Clear();
            Pictures = null;

            DrawCmds?.Clear();
            DrawCmds = null;

            LooseProps?.Clear();
            LooseProps = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        [ByteSize(4)]
        public RoomFlags RoomFlags;
        public sint32 FacesID;
        public RoomID RoomID;
        public sint16 RoomNameOfst;
        public sint16 PictNameOfst;
        public sint16 ArtistNameOfst;
        public sint16 PasswordOfst;
        public sint16 NbrHotspots;
        public sint16 HotspotOfst;
        public sint16 NbrPictures;
        public sint16 PictureOfst;
        public sint16 NbrDrawCmds;
        public sint16 FirstDrawCmd;
        public sint16 NbrPeople;
        public sint16 NbrLProps;
        public sint16 FirstLProp;
        public sint16 Reserved;
        public sint16 LenVars;

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