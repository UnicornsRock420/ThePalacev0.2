using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("mPrp")]
    public partial class BO_PROPMOVE : IProtocolHandler<MSG_PROPMOVE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_PROPMOVE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}