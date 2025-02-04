﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Rooms
{
    [Mnemonic("endr")]
    public partial class BO_ROOMDESCEND : IProtocolHandler<MSG_ROOMDESCEND>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ROOMDESCEND request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}