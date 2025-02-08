using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Network.Client.Communications
{
    [Mnemonic("rmsg")]
    public partial class BO_RMSG : IIntegrationEventHandler<MSG_RMSG>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}