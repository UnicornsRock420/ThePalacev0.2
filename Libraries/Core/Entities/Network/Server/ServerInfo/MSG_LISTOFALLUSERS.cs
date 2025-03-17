using System.Runtime.Serialization;
using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.ServerInfo;
using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.ServerInfo;

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
        if ((Users?.Count ?? 0) < 1) return;

        foreach (var user in Users)
            writer.PalaceSerialize(user, typeof(ListRec), opts);
    }
}