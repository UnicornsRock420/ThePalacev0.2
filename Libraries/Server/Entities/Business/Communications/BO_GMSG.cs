﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Communications;

[Mnemonic("gmsg")]
public class BO_GMSG : IEventHandler<MSG_GMSG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}