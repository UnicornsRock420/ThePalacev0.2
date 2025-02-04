using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("coLs")]
    public partial class BO_SPOTMOVE : IProtocolHandler<MSG_SPOTMOVE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SPOTMOVE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}