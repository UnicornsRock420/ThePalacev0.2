using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using uint32 = uint;
using uint8 = byte;

namespace ThePalace.Core.Entities.Network.Server.Network;

[DynamicSize]
[Mnemonic("blow")]
public class MSG_BLOWTHRU : EventParams, IStructSerializer, IProtocolS2C
{
    public uint8[] Embedded;
    public uint32 PluginTag;

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        throw new NotImplementedException();
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        throw new NotImplementedException();
    }
}