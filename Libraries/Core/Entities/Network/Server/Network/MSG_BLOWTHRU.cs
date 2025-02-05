using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using uint32 = System.UInt32;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [DynamicSize]
    [Mnemonic("blow")]
    public partial class MSG_BLOWTHRU : IStructSerializer, IProtocolS2C
    {
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