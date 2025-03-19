using System.Runtime.Serialization;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Interfaces.Data;
using sint16 = short;
using sint32 = int;

namespace Lib.Core.Entities.Shared.Rooms;

[ByteSize(12)]
public class PictureRec : IStruct
{
    [IgnoreDataMember] public string? Name;

    public sint32 RefCon;
    public sint16 PicID;
    public sint16 PicNameOfst;
    public sint16 TransColor;
    public sint16 Reserved;
}