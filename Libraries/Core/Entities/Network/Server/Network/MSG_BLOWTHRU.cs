using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using uint32 = System.UInt32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("blow")]
    [DynamicSize]
    public partial class MSG_BLOWTHRU : IProtocolS2C, IProtocolSerializer
    {
        public uint32 PluginTag;
        public uint8[] Embedded;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}