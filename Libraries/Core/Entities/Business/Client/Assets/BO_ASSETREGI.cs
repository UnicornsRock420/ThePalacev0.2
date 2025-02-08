using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Assets
{
    [Mnemonic("rAst")]
    public partial class BO_ASSETREGI : IIntegrationEventHandler<MSG_ASSETREGI>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }

        public Task<object?> Handle(object? sender, IEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}