﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Server.Rooms
{
    [DynamicSize]
    [Mnemonic("room")]
    public partial class BO_ROOMDESC : IEventHandler<MSG_ROOMDESC>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}