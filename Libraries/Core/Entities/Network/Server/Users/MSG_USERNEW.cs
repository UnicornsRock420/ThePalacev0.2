using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("nprs")]
public class MSG_USERNEW : EventParams, IProtocolS2C
{
    public UserRec? User;
}