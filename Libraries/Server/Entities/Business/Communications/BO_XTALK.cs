﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Communications;

[DynamicSize(258, 256)]
[Mnemonic("xtlk")]
public class BO_XTALK : IEventHandler<MSG_XTALK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}