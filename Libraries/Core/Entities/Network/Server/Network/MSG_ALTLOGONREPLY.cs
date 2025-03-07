﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Server.Network;

[Mnemonic("rep2")]
public class MSG_ALTLOGONREPLY : EventParams, IProtocolS2C
{
    public RegistrationRec? RegInfo;

    public MSG_ALTLOGONREPLY()
    {
        RegInfo = new RegistrationRec();
    }
}