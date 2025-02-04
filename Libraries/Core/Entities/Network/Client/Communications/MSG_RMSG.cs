using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class MSG_RMSG : IProtocolCommunications, IProtocolC2S
    {
        [CString]
        public string Text { get; set; }
    }
}