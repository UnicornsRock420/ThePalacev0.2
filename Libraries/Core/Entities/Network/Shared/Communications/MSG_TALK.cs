using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Communications;

[Mnemonic("talk")]
public class MSG_TALK : EventParams, IProtocolC2S, IProtocolS2C, IProtocolEcho, ICommunications
{
    [CString(255)] public string? Text { get; set; }
}