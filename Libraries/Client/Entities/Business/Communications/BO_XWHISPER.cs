﻿using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Communications;

[Mnemonic("xwis")]
public class BO_XWHISPER : IEventHandler<MSG_XWHISPER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}