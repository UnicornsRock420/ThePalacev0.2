﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Network.Client.Users
{
    [ByteSize(4)]
    [Mnemonic("kill")]
    public partial class MSG_KILLUSER : IntegrationEvent, IProtocolC2S
    {
        public uint32 TargetID;
    }
}