using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Auth
{
    [Mnemonic("auth")]
    public partial class BO_AUTHENTICATE : IIntegrationEventHandler<MSG_AUTHENTICATE>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}