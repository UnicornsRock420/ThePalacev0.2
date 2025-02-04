using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrD")]
    public partial class BO_USERDESC : IProtocolHandler<MSG_USERDESC>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERDESC request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}