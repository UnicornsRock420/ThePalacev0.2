﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Client.Entities.Business.Shared.Rooms
{
    [Mnemonic("unlo")]
    public partial class BO_DOORUNLOCK : IEventHandler<MSG_DOORUNLOCK>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}