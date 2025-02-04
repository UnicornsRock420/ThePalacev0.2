using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Shared.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Shared.Rooms
{
    [Mnemonic("pLoc")]
    public partial class BO_PICTMOVE : IProtocolHandler<MSG_PICTMOVE>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_PICTMOVE request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}