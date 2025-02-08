using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("prPn")]
    public partial class BO_PROPNEW : IIntegrationEventHandler<MSG_PROPNEW>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}