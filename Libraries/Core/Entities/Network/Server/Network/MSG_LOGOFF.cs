using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using sint32 = int;

namespace Lib.Core.Entities.Network.Server.Network;

[Mnemonic("bye ")]
public class MSG_LOGOFF : EventParams, IProtocolS2C
{
    public sint32 NbrUsers;
}