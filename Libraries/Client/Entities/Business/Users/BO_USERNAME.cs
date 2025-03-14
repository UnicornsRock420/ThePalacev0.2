﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Users;

[DynamicSize(32, 1)]
[Mnemonic("usrN")]
public class BO_USERNAME : IEventHandler<MSG_USERNAME>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}