﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.ServerInfo;

[Mnemonic("sinf")]
public class BO_SERVERINFO : IEventHandler<MSG_SERVERINFO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}