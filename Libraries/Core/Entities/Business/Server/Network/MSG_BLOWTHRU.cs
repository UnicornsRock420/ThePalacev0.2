using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [DynamicSize]
    [Mnemonic("blow")]
    public partial class BO_BLOWTHRU : IProtocolHandler<MSG_BLOWTHRU>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_BLOWTHRU request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}