using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class MSG_TALK : IProtocolC2S, IProtocolS2C, IProtocolCommunications
    {
        [CString]
        public string Text { get; set; }
    }
}