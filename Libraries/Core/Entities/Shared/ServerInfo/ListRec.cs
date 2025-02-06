using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared.ServerInfo
{
    [DynamicSize(40, 12)]
    public partial class ListRec : IStruct
    {
        public ListRec()
        {
            Name = string.Empty;
        }

        public sint32 PrimaryID;
        public sint16 Flags;
        public sint16 RefNum;

        [PString(1, 31, 4)]
        public string Name;
    }

    //[MessagePackObject(true, AllowPrivate = true)]
    //public partial class UserListRec : ListRec
    //{
    //    [IgnoreDataMember]
    //    public sint32 UserID
    //    {
    //        get => PrimaryID;
    //        set => PrimaryID = value;
    //    }
    //    [IgnoreDataMember]
    //    public sint16 RoomID
    //    {
    //        get => RefNum;
    //        set => RefNum = value;
    //    }
    //}

    //[MessagePackObject(true, AllowPrivate = true)]
    //public partial class RoomListRec : ListRec
    //{
    //    [IgnoreDataMember]
    //    public sint32 RoomID
    //    {
    //        get => PrimaryID;
    //        set => PrimaryID = value;
    //    }
    //    [IgnoreDataMember]
    //    public sint16 NbrUsers
    //    {
    //        get => RefNum;
    //        set => RefNum = value;
    //    }
    //}
}