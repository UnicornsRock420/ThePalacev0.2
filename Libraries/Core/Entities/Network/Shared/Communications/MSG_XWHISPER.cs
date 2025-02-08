﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [DynamicSize(260, 258)]
    [Mnemonic("xwis")]
    public partial class MSG_XWHISPER : Entities.Core.EventParams, IProtocolC2S, IProtocolS2C, ICommunications
    {
        public sint32 TargetID;

        [EncryptedString(2, 255)]
        public string? Text { get; set; }
    }
}