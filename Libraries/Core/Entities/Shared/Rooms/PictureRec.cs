using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Data;
using sint16 = short;
using sint32 = int;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(12)]
public class PictureRec : IStruct
{
    [IgnoreDataMember] public string? Name;

    public sint16 PicID;
    public sint16 PicNameOfst;
    public sint32 RefCon;
    public sint16 Reserved;
    public sint16 TransColor;
}