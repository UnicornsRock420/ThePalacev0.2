using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class MSG_TALK : IProtocolC2S, IProtocolS2C, IProtocolCommunications
    {
        public CString Text { get; set; }
    }
}