using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("eprs")]
public class MSG_USEREXIT : EventParams, IProtocolS2C
{
}