using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Network;
using sint16 = short;

namespace Lib.Core.Entities.Network.Shared.Rooms;

[Mnemonic("pLoc")]
public class MSG_PICTMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public sint16 RoomID;
    public sint16 SpotID;
}