using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Shared.Communications;

[DynamicSize(260, 258)]
[Mnemonic("xwis")]
public class MSG_XWHISPER : EventParams, IProtocolC2S, IProtocolS2C, IProtocolEcho, ICommunications
{
    public sint32 TargetID;

    [EncryptedString(2)] public string? Text { get; set; }
}