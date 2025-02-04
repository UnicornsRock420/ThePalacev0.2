using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Client.Users;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Client.Users
{
    [ByteSize(4)]
    [Mnemonic("kill")]
    public partial class BO_KILLUSER : IProtocolHandler<MSG_KILLUSER>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_KILLUSER request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}