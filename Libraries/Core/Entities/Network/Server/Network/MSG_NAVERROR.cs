﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network;

[Mnemonic("sErr")]
public class MSG_NAVERROR : EventParams, IProtocolS2C
{
}