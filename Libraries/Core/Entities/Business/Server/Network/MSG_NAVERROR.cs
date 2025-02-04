using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("sErr")]
    public partial class BO_NAVERROR : IProtocolHandler<MSG_NAVERROR>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_NAVERROR request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}