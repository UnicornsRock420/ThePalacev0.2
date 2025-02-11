﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Network;
using HotSpotID = System.Int16;
using RoomID = System.Int16;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("lock")]
    public partial class MSG_DOORLOCK : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public RoomID RoomID;
        public HotSpotID SpotID;
    }
}