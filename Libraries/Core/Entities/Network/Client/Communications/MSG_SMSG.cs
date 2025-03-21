using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Communications;

[Mnemonic("smsg")]
public class MSG_SMSG : EventParams, IProtocolC2S, ICommunications
{
    [CString(255)] public string? Text { get; set; }
}