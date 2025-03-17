using Lib.Common.Attributes;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Auth;

[Mnemonic("susr")]
public class MSG_SUPERUSER : EventParams, IProtocolC2S
{
    [Str127] public string Password;
}