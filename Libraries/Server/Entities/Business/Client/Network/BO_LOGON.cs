﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Client.Network;

[Mnemonic("regi")]
public class BO_LOGON : IEventHandler<MSG_LOGON>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}