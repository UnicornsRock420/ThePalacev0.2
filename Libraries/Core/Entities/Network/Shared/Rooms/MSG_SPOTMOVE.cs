using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;

namespace ThePalace.Core.Entities.Network.Shared.Rooms;

[Mnemonic("coLs")]
public class MSG_SPOTMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public RoomID RoomID;
    public HotSpotID SpotID;
}