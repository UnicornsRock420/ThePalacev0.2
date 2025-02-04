﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Rooms
{
    [Mnemonic("sRom")]
    public partial class MSG_ROOMSETDESC : IProtocolC2S
    {
        public RoomRec? RoomInfo;
    }
}