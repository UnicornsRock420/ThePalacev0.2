using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Users
{
    [Mnemonic("rprs")]
    public partial class BO_USERLIST : IProtocolHandler<MSG_USERLIST>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_USERLIST request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}