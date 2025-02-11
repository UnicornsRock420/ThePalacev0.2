using ThePalace.Core.Attributes.Serialization;
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
    public partial class MSG_BLOWTHRU : Core.EventParams, IStructSerializer, IProtocolC2S
    {
        public uint32 Flags;
        public uint32 NbrUsers;
        public UserID[] UserIDs; /* iff nbrUsers >= 0 */
        public uint32 PluginTag;
        public uint8[] Embedded;

        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}