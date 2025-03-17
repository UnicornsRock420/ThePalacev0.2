using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Network;

[DynamicSize]
[Mnemonic("HTTP")]
public class MSG_HTTPSERVER : EventParams, IProtocolS2C
{
    [CString] public string? Url;
}