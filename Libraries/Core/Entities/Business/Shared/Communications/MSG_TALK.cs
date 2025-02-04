using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class BO_TALK : IProtocolHandler<MSG_TALK>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_TALK request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}