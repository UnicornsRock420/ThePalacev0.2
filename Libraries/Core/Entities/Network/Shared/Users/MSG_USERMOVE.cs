using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Users;

[Mnemonic("uLoc")]
public class MSG_USERMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
}