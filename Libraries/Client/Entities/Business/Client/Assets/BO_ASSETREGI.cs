﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Client.Assets;

[Mnemonic("rAst")]
public class BO_ASSETREGI : IEventHandler<MSG_ASSETREGI>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}