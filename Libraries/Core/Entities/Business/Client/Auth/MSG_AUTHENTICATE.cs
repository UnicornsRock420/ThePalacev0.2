using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Auth
{
    [Mnemonic("auth")]
    public partial class BO_AUTHENTICATE : IProtocolHandler<MSG_AUTHENTICATE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_AUTHENTICATE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}