using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class MSG_SMSG : Core.EventParams, IProtocolC2S, ICommunications
    {
        [CString(255)]
        public string? Text { get; set; }
    }
}