﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("prPn")]
    [ByteSize(12)]
    public partial class MSG_PROPNEW : EventParams, IProtocolC2S, IProtocolS2C
    {
        public AssetSpec PropSpec;
        public Point Pos;
    }
}