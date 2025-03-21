using Lib.Common.Attributes.Core;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;
using StateID = short;

namespace Lib.Core.Entities.Network.Shared.Rooms;

[Mnemonic("sSta")]
public class MSG_SPOTSTATE : EventParams, IProtocolC2S, IProtocolS2C
{
    public RoomID RoomID;
    public HotSpotID SpotID;
    public StateID StateID;
}