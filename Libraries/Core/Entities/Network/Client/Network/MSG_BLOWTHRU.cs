using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using Lib.Core.Interfaces.Network;
using uint32 = uint;
using uint8 = byte;
using UserID = int;

namespace Lib.Core.Entities.Network.Client.Network;

[DynamicSize]
[Mnemonic("blow")]
public class MSG_BLOWTHRU : EventParams, IStructSerializer, IProtocolC2S
{
    public uint32 Flags;
    public uint32 NbrUsers;
    public UserID[] UserIDs; /* iff nbrUsers >= 0 */
    public uint32 PluginTag;
    public uint8[] Embedded;

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        Flags = reader.ReadUInt32();
        NbrUsers = reader.ReadUInt32();

        UserIDs = new UserID[NbrUsers];
        for (var j = 0; j < NbrUsers; j++) UserIDs[j] = reader.ReadInt32();

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
        writer.WriteUInt32(Flags);
        writer.WriteUInt32((uint)UserIDs.Length);

        foreach (var id in UserIDs) writer.WriteInt32(id);

        writer.WriteUInt32(PluginTag);

        writer.Write(Embedded);

        throw new NotImplementedException(nameof(MSG_BLOWTHRU) + "." + nameof(Serialize));
    }
}