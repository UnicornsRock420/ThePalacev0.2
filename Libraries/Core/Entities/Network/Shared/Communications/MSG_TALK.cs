using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class MSG_TALK : IProtocolC2S, IProtocolS2C, ICommunications
    {
        [CString(255)]
        public string? Text { get; set; }
    }
}