﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.ServerInfo;

[DynamicSize]
[Mnemonic("rLst")]
public class BO_LISTOFALLROOMS : IEventHandler<MSG_LISTOFALLROOMS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}