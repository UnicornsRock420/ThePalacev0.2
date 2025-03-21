using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Rooms;

[DynamicSize]
[Mnemonic("room")]
public class MSG_ROOMDESC : EventParams, IStructSerializer, IProtocolS2C
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