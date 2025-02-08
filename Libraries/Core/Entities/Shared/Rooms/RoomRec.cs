using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using RoomID = System.Int16;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(40)]
    public partial class RoomRec : IStruct
    {
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
    }
}