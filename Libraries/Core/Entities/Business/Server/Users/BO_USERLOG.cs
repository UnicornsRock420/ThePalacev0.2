﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("log ")]
    public partial class BO_USERLOG : IProtocolHandler<MSG_USERLOG>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}