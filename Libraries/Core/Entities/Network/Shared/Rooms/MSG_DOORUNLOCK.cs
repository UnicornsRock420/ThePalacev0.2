using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;

namespace ThePalace.Core.Entities.Network.Shared.Rooms;

[Mnemonic("unlo")]
public class MSG_DOORUNLOCK : EventParams, IProtocolC2S, IProtocolS2C
{
    public RoomID RoomID;
    public HotSpotID SpotID;
}