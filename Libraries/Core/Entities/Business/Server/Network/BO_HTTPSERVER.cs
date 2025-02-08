using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [DynamicSize]
    [Mnemonic("HTTP")]
    public partial class BO_HTTPSERVER : IIntegrationEventHandler<MSG_HTTPSERVER>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}