﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Server.Assets
{
    [Mnemonic("sAst")]
    public partial class MSG_ASSETSEND : IProtocolS2C
    {
        public AssetRec AssetInfo;
    }
}