using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Network
{
    [Mnemonic("navR")]
    public partial class BO_ROOMGOTO : IProtocolHandler<MSG_ROOMGOTO>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ROOMGOTO request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}