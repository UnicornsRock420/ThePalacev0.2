﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Assets
{
    [Mnemonic("sAst")]
    public partial class MSG_ASSETSEND : Core.EventParams, IProtocolS2C
    {
        public AssetRec AssetInfo;
    }
}