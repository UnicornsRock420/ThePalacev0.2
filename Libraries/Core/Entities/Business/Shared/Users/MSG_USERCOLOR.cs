using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [Mnemonic("usrC")]
    public partial class BO_USERCOLOR : IProtocolHandler<MSG_USERCOLOR>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERCOLOR request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}