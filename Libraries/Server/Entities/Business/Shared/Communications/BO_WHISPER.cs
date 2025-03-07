﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Shared.Communications;

[Mnemonic("whis")]
public class BO_WHISPER : IEventHandler<MSG_WHISPER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}