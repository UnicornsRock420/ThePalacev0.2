using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Media
{
    [Mnemonic("sFil")]
    public partial class BO_FILESEND : IProtocolHandler<MSG_FILESEND>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_FILESEND request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}