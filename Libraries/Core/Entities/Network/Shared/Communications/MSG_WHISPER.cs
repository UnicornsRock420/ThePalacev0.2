﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Interfaces.Network;
using sint32 = int;

namespace ThePalace.Core.Entities.Network.Shared.Communications;

[DynamicSize]
[Mnemonic("whis")]
public class MSG_WHISPER : EventParams, IProtocolC2S, IProtocolS2C, IProtocolEcho, ICommunications
{
    public sint32 TargetID;

    [CString(255)] public string? Text { get; set; }
}