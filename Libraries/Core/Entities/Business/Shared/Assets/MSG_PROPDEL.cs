using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Assets
{
    [Mnemonic("dPrp")]
    public partial class BO_PROPDEL : IProtocolHandler<MSG_PROPDEL>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_PROPDEL request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}