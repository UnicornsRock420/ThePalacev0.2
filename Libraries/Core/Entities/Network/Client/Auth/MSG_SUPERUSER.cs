using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Auth;

[Mnemonic("susr")]
public class MSG_SUPERUSER : EventParams, IProtocolC2S
{
    [Str127] public string Password;
}