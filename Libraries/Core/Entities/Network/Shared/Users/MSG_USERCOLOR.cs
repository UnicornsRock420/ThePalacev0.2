using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using sint16 = short;

namespace Lib.Core.Entities.Network.Shared.Users;

[Mnemonic("usrC")]
public class MSG_USERCOLOR : EventParams, IProtocolC2S, IProtocolS2C
{
    public sint16 ColorNbr;
}