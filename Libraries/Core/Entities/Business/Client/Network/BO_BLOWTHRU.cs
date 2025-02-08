using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Network
{
    [DynamicSize]
    [Mnemonic("blow")]
    public partial class BO_BLOWTHRU : IIntegrationEventHandler<MSG_BLOWTHRU>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}