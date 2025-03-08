﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint32 = int;

namespace ThePalace.Core.Entities.Network.Client.Assets;

[Mnemonic("dPrp")]
public class MSG_PROPDEL : EventParams, IProtocolC2S, IProtocolS2C
{
    public sint32 PropNum;
}