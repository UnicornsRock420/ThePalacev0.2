﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("prPn")]
    [ByteSize(12)]
    public partial class MSG_PROPNEW : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C
    {
        public AssetSpec PropSpec;
        public Point Pos;
    }
}