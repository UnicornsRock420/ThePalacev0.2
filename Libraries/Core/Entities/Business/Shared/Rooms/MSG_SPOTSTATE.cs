using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("sSta")]
    public partial class BO_SPOTSTATE : IProtocolHandler<MSG_SPOTSTATE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SPOTSTATE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}