using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Users
{
    [DynamicSize(32, 1)]
    [Mnemonic("usrN")]
    public partial class BO_USERNAME : IProtocolHandler<MSG_USERNAME>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERNAME request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}