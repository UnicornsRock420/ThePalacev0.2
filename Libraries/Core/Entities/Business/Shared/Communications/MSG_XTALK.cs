﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Communications
{
    [DynamicSize(258, 256)]
    [Mnemonic("xtlk")]
    public partial class BO_XTALK : IProtocolHandler<MSG_XTALK>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_XTALK request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}