using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint16 = short;

namespace ThePalace.Core.Entities.Network.Shared.Users;

[Mnemonic("usrC")]
public class MSG_USERCOLOR : EventParams, IProtocolC2S, IProtocolS2C
{
    public sint16 ColorNbr;
}