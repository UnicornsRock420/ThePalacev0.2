using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("sInf")]
    public partial class BO_EXTENDEDINFO : IProtocolHandler<MSG_EXTENDEDINFO>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_EXTENDEDINFO request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}