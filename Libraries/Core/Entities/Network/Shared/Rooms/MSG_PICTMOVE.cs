using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;
using sint16 = short;

namespace ThePalace.Core.Entities.Network.Shared.Rooms;

[Mnemonic("pLoc")]
public class MSG_PICTMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public sint16 RoomID;
    public sint16 SpotID;
}