using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Network
{
    [Mnemonic("regi")]
    public partial class BO_LOGON : IProtocolHandler<MSG_LOGON>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_LOGON request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}