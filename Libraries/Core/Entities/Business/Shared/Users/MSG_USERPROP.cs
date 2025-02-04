using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrP")]
    public partial class BO_USERPROP : IProtocolHandler<MSG_USERPROP>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERPROP request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}