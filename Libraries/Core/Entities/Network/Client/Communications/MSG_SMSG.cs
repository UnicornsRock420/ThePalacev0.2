using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class MSG_SMSG : IProtocolCommunications, IProtocolC2S
    {
        public CString Text { get; set; }
    }
}