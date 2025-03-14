﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Network;

[Mnemonic("sInf")]
public class BO_EXTENDEDINFO : IEventHandler<MSG_EXTENDEDINFO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}