using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("qAst")]
    public partial class BO_ASSETQUERY : IProtocolHandler<MSG_ASSETQUERY>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ASSETQUERY request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}