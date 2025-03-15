using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network;

[DynamicSize]
[Mnemonic("HTTP")]
public class MSG_HTTPSERVER : EventParams, IProtocolS2C
{
    [CString] public string? Url;
}