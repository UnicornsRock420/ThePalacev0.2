using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("opSn")]
    public partial class BO_SPOTNEW : IProtocolHandler<MSG_SPOTNEW>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SPOTNEW request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}