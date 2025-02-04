using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("whis")]
    public partial class MSG_WHISPER : IProtocolC2S, IProtocolS2C, IProtocolCommunications
    {
        public sint32 TargetID;

        [CString]
        public string? Text { get; set; }
    }
}