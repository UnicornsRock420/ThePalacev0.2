﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Shared.Users;

[Mnemonic("usrP")]
public class MSG_USERPROP : EventParams, IProtocolC2S, IProtocolS2C
{
    public AssetSpec[] AssetSpec;
}