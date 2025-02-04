﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class MSG_SMSG : IProtocolCommunications, IProtocolC2S
    {
        [CString]
        public string Text { get; set; }
    }
}