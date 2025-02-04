﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Shared.Communications
{
    [Mnemonic("xwis")]
    [DynamicSize(260, 258)]
    public partial class MSG_XWHISPER : IProtocolC2S, IProtocolS2C, ICommunications
    {
        public sint32 TargetID;

        [EncryptedString(2, 255)]
        public string? Text { get; set; }
    }
}