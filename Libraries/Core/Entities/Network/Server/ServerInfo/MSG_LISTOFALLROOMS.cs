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
        if ((Rooms?.Count ?? 0) > 0)
            foreach (var room in Rooms)
                writer.PalaceSerialize(room, typeof(ListRec), opts);
    }
}