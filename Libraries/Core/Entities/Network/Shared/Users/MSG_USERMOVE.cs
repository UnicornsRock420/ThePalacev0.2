using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Users;

[Mnemonic("uLoc")]
public class MSG_USERMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
}