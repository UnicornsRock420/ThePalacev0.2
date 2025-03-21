using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Shared.Communications;

[DynamicSize]
[Mnemonic("whis")]
public class MSG_WHISPER : EventParams, IProtocolC2S, IProtocolS2C, IProtocolEcho, ICommunications
{
    public sint32 TargetID;

    [CString(255)] public string? Text { get; set; }
}