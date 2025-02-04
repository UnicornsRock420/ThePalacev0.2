using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Communications
{
    [Mnemonic("whis")]
    public partial class BO_WHISPER : IProtocolHandler<MSG_WHISPER>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_WHISPER request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}