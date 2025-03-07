﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Network.Entities.Unused
{
    [Mnemonic("wprs")]
    public partial class MSG_USERENTER : EventParams, IProtocol
    {
        public UserRec? User;
    }
}