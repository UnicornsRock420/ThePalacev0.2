﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("sRom")]
    public partial class BO_ROOMSETDESC : IEventHandler<MSG_ROOMSETDESC>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}