using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("draw")]

    public partial class BO_DRAW : IProtocolHandler<MSG_DRAW>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_DRAW request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}