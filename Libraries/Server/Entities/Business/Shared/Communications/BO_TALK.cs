﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class BO_TALK : IEventHandler<MSG_TALK>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}