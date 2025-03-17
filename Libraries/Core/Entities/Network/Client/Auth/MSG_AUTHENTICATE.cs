using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Client.Auth;

[Mnemonic("auth")]
public class MSG_AUTHENTICATE : EventParams, IProtocolC2S
{
}