﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network
{
    [Mnemonic("rep2")]
    public partial class MSG_ALTLOGONREPLY : IProtocolS2C
    {
        public RegistrationRec? RegInfo;
    }
}