using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Assets
{
    [Mnemonic("sAst")]
    public partial class BO_ASSETSEND : IProtocolHandler<MSG_ASSETSEND>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ASSETSEND request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}