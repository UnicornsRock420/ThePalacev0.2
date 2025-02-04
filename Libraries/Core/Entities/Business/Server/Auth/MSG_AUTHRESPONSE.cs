using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Auth
{
    [Mnemonic("autr")]
    public partial class BO_AUTHRESPONSE : IProtocolHandler<MSG_AUTHRESPONSE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_AUTHRESPONSE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}