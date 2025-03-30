using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("log ")]
public class MSG_USERLOG : EventParams, IProtocolS2C
{
    public sint32 NbrUsers;
}