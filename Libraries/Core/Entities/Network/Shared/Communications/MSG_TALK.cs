using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Communications;

[Mnemonic("talk")]
public partial class MSG_TALK : EventParams, IProtocolC2S, IProtocolS2C, IProtocolEcho, ICommunications
{
    [CString(255)]
    public string? Text { get; set; }
}