﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Users;

[Mnemonic("usrD")]
public class BO_USERDESC : IEventHandler<MSG_USERDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}