using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Network
{
    [Mnemonic("tiyr")]
    public partial class BO_TIYID : IProtocolHandler<MSG_TIYID>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_TIYID request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}