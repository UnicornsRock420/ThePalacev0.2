﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("sErr")]
    public partial class MSG_NAVERROR : Core.EventParams, IProtocolS2C
    {
    }
}