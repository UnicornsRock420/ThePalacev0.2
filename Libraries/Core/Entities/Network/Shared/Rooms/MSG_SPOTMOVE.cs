﻿using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Network;
using HotSpotID = short;
using RoomID = short;

namespace Lib.Core.Entities.Network.Shared.Rooms;

[Mnemonic("coLs")]
public class MSG_SPOTMOVE : EventParams, IProtocolC2S, IProtocolS2C
{
    public Point Pos;
    public RoomID RoomID;
    public HotSpotID SpotID;
}