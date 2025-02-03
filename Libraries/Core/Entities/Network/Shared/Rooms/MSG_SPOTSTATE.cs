﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using HotSpotID = System.Int16;
using RoomID = System.Int16;
using StateID = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("sSta")]
    public partial class MSG_SPOTSTATE : IProtocolC2S, IProtocolS2C
    {
        public RoomID RoomID;
        public HotSpotID SpotID;
        public StateID StateID;
    }
}