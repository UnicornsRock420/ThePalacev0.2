using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("opSd")]
    public partial class BO_SPOTDEL : IProtocolHandler<MSG_SPOTDEL>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SPOTDEL request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}