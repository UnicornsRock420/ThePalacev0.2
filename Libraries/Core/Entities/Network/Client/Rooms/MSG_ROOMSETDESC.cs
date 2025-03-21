using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Auth;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Rooms;

[Mnemonic("sRom")]
[Restricted]
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