﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("coLs")]
    public partial class BO_SPOTMOVE : IEventHandler<MSG_SPOTMOVE>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}