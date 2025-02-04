using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("rep2")]
    public partial class BO_ALTLOGONREPLY : IProtocolHandler<MSG_ALTLOGONREPLY>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ALTLOGONREPLY request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}