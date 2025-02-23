using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wmsg")]
    public partial class MSG_WMSG : EventParams, IProtocol, ICommunications
    {
        [CString]
        public string Text { get; set; }
    }
}