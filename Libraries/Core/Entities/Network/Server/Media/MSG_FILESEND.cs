﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Media
{
    [Mnemonic("sFil")]
    public partial class MSG_FILESEND : EventParams, IProtocolC2S
    {
    }
}