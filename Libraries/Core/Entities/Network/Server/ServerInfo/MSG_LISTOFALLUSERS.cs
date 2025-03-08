using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint32 = int;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo;

[DynamicSize]
[Mnemonic("uLst")]
public class MSG_LISTOFALLUSERS : EventParams, IStructRefNum, IStructSerializer, IProtocolS2C
{
    public List<ListRec>? Users;

    [IgnoreDataMember]
    public sint32 RefNum
    {
        get => Users?.Count ?? 0;
        set
        {
            if (value > 0) return;

            Users = [];
        }
    }

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        Users = [];

        while (reader.Length - reader.Position >= 12)
        {
            var user = new ListRec();
            reader.PalaceDeserialize(user, typeof(ListRec), opts);
            Users.Add(user);
        }
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        if ((Users?.Count ?? 0) > 0)
            foreach (var user in Users)
                writer.PalaceSerialize(user, typeof(ListRec), opts);
    }
}