using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("durl")]
    public partial class BO_DISPLAYURL : IProtocolHandler<MSG_DISPLAYURL>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_DISPLAYURL request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}