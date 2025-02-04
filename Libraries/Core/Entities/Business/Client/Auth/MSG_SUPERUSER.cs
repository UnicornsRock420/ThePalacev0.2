using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Auth
{
    [Mnemonic("susr")]
    public partial class BO_SUPERUSER : IProtocolHandler<MSG_SUPERUSER>
    {
        public Task<object> Handle(int? sourceID, int refNum, MSG_SUPERUSER request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}