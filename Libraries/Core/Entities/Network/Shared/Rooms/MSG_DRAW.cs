﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Rooms
{
    [Mnemonic("draw")]
    public partial class MSG_DRAW : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public DrawCmdRec? DrawCmdInfo;
    }
}