﻿using System.Runtime.Serialization;
using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Users;

[Mnemonic("uSta")]
public class MSG_USERSTATUS : EventParams, IProtocolS2C
{
    public UserFlags Flags;

    [IgnoreDataMember] public Guid Hash;
}