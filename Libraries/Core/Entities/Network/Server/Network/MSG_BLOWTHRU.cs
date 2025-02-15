using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using uint32 = System.UInt32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [DynamicSize]
    [Mnemonic("blow")]
    public partial class MSG_BLOWTHRU : EventsBus.EventParams, IStructSerializer, IProtocolS2C
    {
        public uint32 PluginTag;
        public uint8[] Embedded;

        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}