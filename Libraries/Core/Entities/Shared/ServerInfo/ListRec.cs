using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Data;
using sint16 = short;
using sint32 = int;

namespace ThePalace.Core.Entities.Shared.ServerInfo;

[DynamicSize(40, 12)]
public class ListRec : IStruct
{
    public sint16 Flags;

    [PString(1, 31, 4)] public string Name;

    public sint32 PrimaryID;
    public sint16 RefNum;

    public ListRec()
    {
        Name = string.Empty;
    }
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