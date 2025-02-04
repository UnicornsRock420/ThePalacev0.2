﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Network;
using ThePalace.Core.Types;

namespace ThePalace.Core.Entities.Network.Shared.Assets
{
    [Mnemonic("prPn")]
    [ByteSize(12)]
    public partial class MSG_PROPNEW : IProtocolC2S, IProtocolS2C
    {
        public AssetSpec PropSpec;
        public Point Pos;
    }
}