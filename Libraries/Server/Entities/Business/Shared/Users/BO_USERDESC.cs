﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Shared.Users
{
    [Mnemonic("usrD")]
    public partial class BO_USERDESC : IEventHandler<MSG_USERDESC>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}