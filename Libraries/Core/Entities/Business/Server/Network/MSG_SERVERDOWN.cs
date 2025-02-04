using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [ByteSize(4)]
    [Mnemonic("down")]
    public partial class BO_SERVERDOWN : IProtocolHandler<MSG_SERVERDOWN>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_SERVERDOWN request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}