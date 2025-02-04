using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("xtlk")]
    [DynamicSize(258, 256)]
    public partial class MSG_XTALK : IProtocolC2S, IProtocolS2C, IProtocolCommunications
    {
        [EncryptedString(2, 254)]
        public string? Text { get; set; }
    }
}