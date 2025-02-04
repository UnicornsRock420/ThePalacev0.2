using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrF")]
    public partial class BO_USERFACE : IProtocolHandler<MSG_USERFACE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERFACE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}