using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class BO_RMSG : IProtocolHandler<MSG_RMSG>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_RMSG request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}