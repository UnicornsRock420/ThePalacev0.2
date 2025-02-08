using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Client.Auth
{
    [Mnemonic("susr")]
    public partial class BO_SUPERUSER : IIntegrationEventHandler<MSG_SUPERUSER>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}