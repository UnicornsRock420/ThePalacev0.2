using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Communications
{
    [Mnemonic("gmsg")]
    public partial class BO_GMSG : IIntegrationEventHandler<MSG_GMSG>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}