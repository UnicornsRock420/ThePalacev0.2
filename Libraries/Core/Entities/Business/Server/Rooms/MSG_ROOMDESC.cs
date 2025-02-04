using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Rooms
{
    [DynamicSize]
    [Mnemonic("room")]
    public partial class BO_ROOMDESC : IProtocolHandler<MSG_ROOMDESC>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ROOMDESC request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}