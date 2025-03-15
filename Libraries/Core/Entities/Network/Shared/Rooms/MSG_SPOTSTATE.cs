using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;
using StateID = short;

namespace ThePalace.Core.Entities.Network.Shared.Rooms;

[Mnemonic("sSta")]
public class MSG_SPOTSTATE : EventParams, IProtocolC2S, IProtocolS2C
{
    public RoomID RoomID;
    public HotSpotID SpotID;
    public StateID StateID;
}