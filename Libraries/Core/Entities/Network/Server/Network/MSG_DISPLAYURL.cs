using Lib.Common.Attributes;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("durl")]
public class MSG_DISPLAYURL : EventParams, IProtocolS2C
{
    [CString] public string? Url;
}