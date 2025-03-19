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
    public uint32 PluginTag;
    public uint8[] Embedded;

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        PluginTag = reader.ReadUInt32();

        var buffer = new byte[reader.Length - reader.Position];
        if (buffer.Length > 0)
        {
            reader.Read(buffer, 0, buffer.Length);
            Embedded = buffer;
        }

        throw new NotImplementedException(nameof(MSG_BLOWTHRU) + "." + nameof(Deserialize));
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteUInt32(PluginTag);

        writer.Write(Embedded);

        throw new NotImplementedException(nameof(MSG_BLOWTHRU) + "." + nameof(Serialize));
    }
}