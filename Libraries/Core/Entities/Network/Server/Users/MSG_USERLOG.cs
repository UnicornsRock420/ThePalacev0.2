using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using uint32 = uint;

namespace Lib.Core.Entities.Network.Server.Users;

[Mnemonic("log ")]
public class MSG_USERLOG : EventParams, IProtocolS2C
{
    public uint32 NbrUsers;
}