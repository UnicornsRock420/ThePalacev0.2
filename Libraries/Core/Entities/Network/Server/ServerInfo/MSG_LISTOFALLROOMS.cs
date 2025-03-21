using System.Runtime.Serialization;
using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.ServerInfo;
using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.ServerInfo;

[DynamicSize]
[Mnemonic("rLst")]
public class MSG_LISTOFALLROOMS : EventParams, IStructRefNum, IStructSerializer, IProtocolS2C
{
    public List<ListRec>? Rooms;

    [IgnoreDataMember]
    public sint32 RefNum
    {
        get => Rooms?.Count ?? 0;
        set
        {
            if (value > 0) return;

            Rooms = [];
        }
    }

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        Rooms = [];

        while (reader.Length - reader.Position >= 12)
        {
            var room = new ListRec();
            reader.PalaceDeserialize(room, typeof(ListRec), opts);
            Rooms.Add(room);
        }
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        if ((Rooms?.Count ?? 0) < 1) return;

        foreach (var room in Rooms)
            writer.PalaceSerialize(room, typeof(ListRec), opts);
    }
}