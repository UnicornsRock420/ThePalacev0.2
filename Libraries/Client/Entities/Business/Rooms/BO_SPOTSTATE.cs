﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Rooms;

[Mnemonic("sSta")]
public class BO_SPOTSTATE : IEventHandler<MSG_SPOTSTATE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}