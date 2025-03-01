using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(12)]
public partial class PictureRec : IStruct
{
    public sint32 RefCon;
    public sint16 PicID;
    public sint16 PicNameOfst;
    public sint16 TransColor;
    public sint16 Reserved;

    [IgnoreDataMember]
    public string? Name;
}