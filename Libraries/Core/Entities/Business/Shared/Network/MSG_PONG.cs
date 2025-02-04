using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("pong")]
    public partial class BO_PONG : IProtocolHandler<MSG_PONG>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_PONG request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}