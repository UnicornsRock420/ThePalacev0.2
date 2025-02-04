using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("eprs")]
    public partial class BO_USEREXIT : IProtocolHandler<MSG_USEREXIT>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USEREXIT request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}