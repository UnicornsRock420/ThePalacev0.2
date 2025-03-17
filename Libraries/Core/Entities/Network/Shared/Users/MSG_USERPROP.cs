﻿using Lib.Common.Attributes;
using Lib.Core.Entities.EventsBus;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Users;

[Mnemonic("usrP")]
public class MSG_USERPROP : EventParams, IProtocolC2S, IProtocolS2C
{
    public AssetSpec[] AssetSpec;
}