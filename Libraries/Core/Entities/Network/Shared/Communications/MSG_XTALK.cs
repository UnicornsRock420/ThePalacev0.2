using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("xtlk")]
    [DynamicSize]
    public partial class MSG_XTALK : IProtocolC2S, IProtocolS2C, IProtocolCommunications, IProtocolSerializer
    {
        public EncryptedString Text { get; set; }

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            var buffer = new byte[reader.Length];
            reader.Read(buffer, 0, buffer.Length);

            this.Text = new EncryptedString(buffer, EncryptedStringOptions.None);
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.Write(this.Text.Value);
        }
    }
}