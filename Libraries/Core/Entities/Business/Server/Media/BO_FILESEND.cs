﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Server.Media
{
    [Mnemonic("sFil")]
    public partial class BO_FILESEND : IEventHandler<MSG_FILESEND>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}