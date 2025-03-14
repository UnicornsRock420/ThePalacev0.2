﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Rooms;

[Mnemonic("nRom")]
public class BO_ROOMNEW : IEventHandler<MSG_ROOMNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}