﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.ServerInfo;

[ByteSize(0)]
[Mnemonic("uLst")]
public class MSG_LISTOFALLUSERS : EventParams, IProtocolC2S
{
}