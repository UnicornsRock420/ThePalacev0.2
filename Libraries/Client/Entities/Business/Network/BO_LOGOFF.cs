﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Network;

[Mnemonic("bye ")]
public class BO_LOGOFF : IEventHandler<MSG_LOGOFF>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}