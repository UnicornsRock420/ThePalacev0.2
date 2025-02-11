﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Users
{
    [Mnemonic("uLoc")]
    public partial class MSG_USERMOVE : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public Point Pos;
    }
}