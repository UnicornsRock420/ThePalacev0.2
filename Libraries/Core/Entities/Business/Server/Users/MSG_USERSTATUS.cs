using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("uSta")]
    public partial class BO_USERSTATUS : IProtocolHandler<MSG_USERSTATUS>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERSTATUS request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}