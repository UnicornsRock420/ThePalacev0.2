using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("eprs")]
public class MSG_USEREXIT : EventParams, IProtocolS2C
{
}