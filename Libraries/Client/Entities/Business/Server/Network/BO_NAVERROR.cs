﻿using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Server.Network;

[Mnemonic("sErr")]
public partial class BO_NAVERROR : IEventHandler<MSG_NAVERROR>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}