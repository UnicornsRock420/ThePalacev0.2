using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Auth;

[Mnemonic("auth")]
public class MSG_AUTHENTICATE : EventParams, IProtocolC2S
{
}