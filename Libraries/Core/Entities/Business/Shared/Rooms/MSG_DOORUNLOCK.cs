using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("unlo")]
    public partial class BO_DOORUNLOCK : IProtocolHandler<MSG_DOORUNLOCK>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_DOORUNLOCK request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}