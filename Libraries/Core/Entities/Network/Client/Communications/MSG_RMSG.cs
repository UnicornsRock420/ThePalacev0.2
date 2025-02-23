using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class MSG_RMSG : EventParams, IProtocolC2S, ICommunications
    {
        [CString(255)]
        public string? Text { get; set; }
    }
}