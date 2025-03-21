using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using sint16 = short;

namespace Lib.Core.Entities.Network.Shared.Users;

[Mnemonic("usrC")]
public class MSG_USERCOLOR : EventParams, IProtocolC2S, IProtocolS2C
{
    public sint16 ColorNbr;
}