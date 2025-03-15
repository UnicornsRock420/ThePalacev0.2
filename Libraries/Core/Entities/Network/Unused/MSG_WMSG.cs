using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wmsg")]
    public class MSG_WMSG : EventParams, IProtocol, ICommunications
    {
        [CString]
        public string Text { get; set; }
    }
}