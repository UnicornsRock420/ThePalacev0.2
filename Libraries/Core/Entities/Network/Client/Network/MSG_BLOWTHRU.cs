using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using uint32 = System.UInt32;
using uint8 = System.Byte;
using UserID = System.Int32;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [DynamicSize]
    [Mnemonic("blow")]
    public partial class MSG_BLOWTHRU : EventParams, IStructSerializer, IProtocolC2S
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
            for (var j = 0; j < NbrUsers; j++)
            {
                UserIDs[j] = reader.ReadInt32();
            }

            PluginTag = reader.ReadUInt32();

            var buffer = new byte[reader.Length - reader.Position];
            reader.Read(buffer, 0, buffer.Length);
            Embedded = buffer;

            throw new NotImplementedException();
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteUInt32(Flags);
            writer.WriteUInt32((uint)UserIDs.Length);

            foreach (var id in UserIDs)
            {
                writer.WriteInt32(id);
            }

            writer.WriteUInt32(PluginTag);

            writer.Write(Embedded);

            throw new NotImplementedException();
        }
    }
}