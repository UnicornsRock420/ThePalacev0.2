using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using uint32 = uint;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("log ")]
public class MSG_USERLOG : EventParams, IProtocolS2C
{
    public uint32 NbrUsers;
}