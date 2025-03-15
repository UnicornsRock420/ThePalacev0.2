using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users;

[Mnemonic("nprs")]
public class MSG_USERNEW : EventParams, IProtocolS2C
{
    public UserRec? User;
}