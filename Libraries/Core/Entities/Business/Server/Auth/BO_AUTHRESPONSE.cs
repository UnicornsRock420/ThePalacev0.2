using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Auth
{
    [Mnemonic("autr")]
    public partial class BO_AUTHRESPONSE : IIntegrationEventHandler<MSG_AUTHRESPONSE>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}