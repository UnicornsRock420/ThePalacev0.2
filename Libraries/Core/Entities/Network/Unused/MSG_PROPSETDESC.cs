﻿using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("sPrp")]
    public class MSG_PROPSETDESC : EventParams, IProtocol
    {
    }
}