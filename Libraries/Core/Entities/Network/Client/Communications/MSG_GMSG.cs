using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("gmsg")]
    public partial class MSG_GMSG : IProtocolCommunications, IProtocolC2S
    {
        [CString]
        public string Text { get; set; }
    }
}