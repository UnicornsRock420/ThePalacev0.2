using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("uLoc")]
    public partial class BO_USERMOVE : IProtocolHandler<MSG_USERMOVE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERMOVE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}