﻿using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;

namespace Lib.Core.Entities.Network.Shared.Rooms;

[Mnemonic("lock")]
public class MSG_DOORLOCK : EventParams, IProtocolC2S, IProtocolS2C
{
    public RoomID RoomID;
    public HotSpotID SpotID;
}