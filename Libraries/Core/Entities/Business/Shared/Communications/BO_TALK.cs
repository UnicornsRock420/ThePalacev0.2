using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Communications
{
    [Mnemonic("talk")]
    public partial class BO_TALK : IIntegrationEventHandler<MSG_TALK>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}