using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("ping")]
    public partial class BO_PING : IProtocolHandler<MSG_PING>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_PING request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}