using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class MSG_TALK : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C, ICommunications
    {
        [CString(255)]
        public string? Text { get; set; }
    }
}