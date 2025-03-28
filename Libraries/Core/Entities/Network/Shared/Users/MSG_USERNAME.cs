﻿using Lib.Common.Attributes.Core;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Entities.EventArgs;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Network.Shared.Users;

[DynamicSize(32, 1)]
[Mnemonic("usrN")]
public class MSG_USERNAME : EventParams, IProtocolC2S, IProtocolS2C
{
    [Str31] public string? Name;
}