using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Communications;

[DynamicSize(258, 256)]
[Mnemonic("xtlk")]
public class MSG_XTALK : EventParams, IProtocolC2S, IProtocolS2C, IProtocolEcho, ICommunications
{
    [EncryptedString(2)] public string? Text { get; set; }
}