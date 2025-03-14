﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Auth;

[Mnemonic("susr")]
public class MSG_SUPERUSER : EventParams, IProtocolC2S
{
    [Str127] public string Password;
}