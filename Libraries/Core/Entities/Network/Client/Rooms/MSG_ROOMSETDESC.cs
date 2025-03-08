using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms;

[Mnemonic("sRom")]
public class MSG_ROOMSETDESC : EventParams, IStructSerializer, IProtocolC2S
{
    public RoomDesc? RoomInfo;

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        RoomInfo.Deserialize(reader, opts);
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        RoomInfo.Serialize(writer, opts);
    }
}