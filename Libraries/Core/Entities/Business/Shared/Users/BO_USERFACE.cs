﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Events;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrF")]
    public partial class BO_USERFACE : IProtocolHandler<MSG_USERFACE>
    {
        public Task<object?> Handle(ProtocolEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}