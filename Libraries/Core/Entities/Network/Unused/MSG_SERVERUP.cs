﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("init")]
    public partial class MSG_SERVERUP : EventParams, IProtocol
    {
    }
}