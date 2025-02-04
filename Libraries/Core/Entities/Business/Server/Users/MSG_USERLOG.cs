using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("log ")]
    public partial class BO_USERLOG : IProtocolHandler<MSG_USERLOG>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERLOG request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}