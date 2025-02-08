﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("pong")]
    public partial class BO_PONG : IEventHandler<MSG_PONG>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}