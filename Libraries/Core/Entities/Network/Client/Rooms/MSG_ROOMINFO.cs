﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("ofNr")]
    public partial class MSG_ROOMINFO : IProtocolC2S
    {
        public RoomRec? RoomInfo;
    }
}