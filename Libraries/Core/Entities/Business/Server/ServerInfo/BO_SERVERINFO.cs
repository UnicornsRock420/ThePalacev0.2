﻿using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [Mnemonic("sinf")]
    public partial class BO_SERVERINFO : IEventHandler<MSG_SERVERINFO>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}