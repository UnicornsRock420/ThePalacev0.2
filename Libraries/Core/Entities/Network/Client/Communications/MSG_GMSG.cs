using Lib.Common.Attributes;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Communications;

[Mnemonic("gmsg")]
public class MSG_GMSG : EventParams, IProtocolC2S, ICommunications
{
    [CString(255)] public string? Text { get; set; }
}