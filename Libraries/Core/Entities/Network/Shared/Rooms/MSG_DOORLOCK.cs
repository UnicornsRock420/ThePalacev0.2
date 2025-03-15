using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;

namespace ThePalace.Core.Entities.Network.Shared.Rooms;

[Mnemonic("lock")]
public class MSG_DOORLOCK : EventParams, IProtocolC2S, IProtocolS2C
{
    public RoomID RoomID;
    public HotSpotID SpotID;
}