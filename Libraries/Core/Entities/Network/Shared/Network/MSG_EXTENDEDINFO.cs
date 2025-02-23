﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Network
{
    [Mnemonic("sInf")]
    public partial class MSG_EXTENDEDINFO : EventParams, IProtocolC2S, IProtocolS2C
    {
        public ServerExtInfoTypes Flags;
    }
}