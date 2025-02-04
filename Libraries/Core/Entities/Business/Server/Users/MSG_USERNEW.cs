using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("nprs")]
    public partial class BO_USERNEW : IProtocolHandler<MSG_USERNEW>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERNEW request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}