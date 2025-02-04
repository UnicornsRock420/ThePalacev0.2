using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("sRom")]
    public partial class BO_ROOMSETDESC : IProtocolHandler<MSG_ROOMSETDESC>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ROOMSETDESC request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}