using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class MSG_RMSG : IProtocolCommunications, IProtocolC2S
    {
        public CString Text { get; set; }
    }
}