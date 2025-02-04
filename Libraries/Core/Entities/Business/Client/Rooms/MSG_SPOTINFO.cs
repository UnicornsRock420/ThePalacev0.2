using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("ofNs")]
    public partial class BO_SPOTINFO : IProtocolHandler<MSG_SPOTINFO>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SPOTINFO request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}