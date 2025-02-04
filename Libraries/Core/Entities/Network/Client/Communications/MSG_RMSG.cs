using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class MSG_RMSG : ICommunications, IProtocolC2S
    {
        [CString(255)]
        public string Text { get; set; }
    }
}