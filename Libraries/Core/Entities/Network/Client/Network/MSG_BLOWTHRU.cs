using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using uint32 = System.UInt32;
using uint8 = System.Byte;
using UserID = System.Int32;

namespace ThePalace.Core.Entities.Network.Client.Network
{
    [Mnemonic("blow")]
    [DynamicSize]
    public partial class MSG_BLOWTHRU : IProtocolC2S, IStructSerializer
    {
        public uint32 Flags;
        public uint32 NbrUsers;
        public UserID[] UserIDs; /* iff nbrUsers >= 0 */
        public uint32 PluginTag;
        public uint8[] Embedded;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}