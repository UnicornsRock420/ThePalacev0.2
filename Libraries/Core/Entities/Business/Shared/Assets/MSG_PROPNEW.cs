using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("prPn")]
    public partial class BO_PROPNEW : IProtocolHandler<MSG_PROPNEW>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_PROPNEW request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}