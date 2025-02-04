using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [DynamicSize]
    [Mnemonic("HTTP")]
    public partial class BO_HTTPSERVER : IProtocolHandler<MSG_HTTPSERVER>
    {
        public Task<object?> Handle(int? sourceID, int refNum, MSG_HTTPSERVER request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}