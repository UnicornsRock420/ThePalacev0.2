using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Rooms
{
    [Mnemonic("nRom")]
    public partial class BO_ROOMNEW : IProtocolHandler<MSG_ROOMNEW>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_ROOMNEW request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}