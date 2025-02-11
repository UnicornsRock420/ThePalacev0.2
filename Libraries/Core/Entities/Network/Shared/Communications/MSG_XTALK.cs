using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [DynamicSize(258, 256)]
    [Mnemonic("xtlk")]
    public partial class MSG_XTALK : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C, ICommunications
    {
        [EncryptedString(2, 255)]
        public string? Text { get; set; }
    }
}