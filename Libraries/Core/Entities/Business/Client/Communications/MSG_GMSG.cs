using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Communications
{
    [Mnemonic("gmsg")]
    public partial class BO_GMSG : IProtocolHandler<MSG_GMSG>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_GMSG request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}