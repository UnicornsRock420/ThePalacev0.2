using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Network
{
    [Mnemonic("bye ")]
    public partial class BO_LOGOFF : IProtocolHandler<MSG_LOGOFF>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_LOGOFF request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}