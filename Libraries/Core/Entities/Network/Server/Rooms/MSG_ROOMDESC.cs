using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Rooms
{
    [DynamicSize]
    [Mnemonic("room")]
    public partial class MSG_ROOMDESC : EventParams, IStructSerializer, IProtocolS2C
    {
        public RoomDesc? RoomInfo;

        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None) =>
            RoomInfo.Deserialize(reader, opts);

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None) =>
            RoomInfo.Serialize(writer, opts);
    }
}