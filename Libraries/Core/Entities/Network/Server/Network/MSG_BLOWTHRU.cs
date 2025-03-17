using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;
using uint32 = uint;
using uint8 = byte;

namespace Lib.Core.Entities.Network.Server.Network;

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