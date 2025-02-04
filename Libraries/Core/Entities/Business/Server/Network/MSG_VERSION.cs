using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("vers")]
    public partial class BO_VERSION : IProtocolHandler<MSG_VERSION>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_VERSION request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}