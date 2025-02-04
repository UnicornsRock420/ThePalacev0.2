using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("ofNr")]
    public partial class BO_ROOMINFO : IProtocolHandler<MSG_ROOMINFO>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ROOMINFO request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}