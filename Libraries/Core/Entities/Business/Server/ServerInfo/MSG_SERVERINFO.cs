﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.ServerInfo
{
    [Mnemonic("sinf")]
    public partial class BO_SERVERINFO : IProtocolHandler<MSG_SERVERINFO>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SERVERINFO request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}