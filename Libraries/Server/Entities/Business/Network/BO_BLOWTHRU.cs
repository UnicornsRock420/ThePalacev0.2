﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Network;

[DynamicSize]
[Mnemonic("blow")]
public class BO_BLOWTHRU : IEventHandler<MSG_BLOWTHRU>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}