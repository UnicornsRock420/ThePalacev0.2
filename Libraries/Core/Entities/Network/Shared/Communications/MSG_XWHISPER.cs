using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("xwis")]
    [DynamicSize]
    public partial class MSG_XWHISPER : IProtocolC2S, IProtocolS2C, IProtocolCommunications, IProtocolSerializer
    {
        public sint32 TargetID;
        public EncryptedString Text { get; set; }

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.TargetID = reader.ReadInt32();

            var buffer = new byte[reader.Length];
            reader.Read(buffer, 0, buffer.Length);

            this.Text = new EncryptedString(buffer, EncryptedStringOptions.None);
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt32(this.TargetID);
            writer.Write(this.Text.Value);
        }
    }
}