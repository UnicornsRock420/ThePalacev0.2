using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Communications
{
    [DynamicSize(258, 256)]
    [Mnemonic("xtlk")]
    public partial class BO_XTALK : IIntegrationEventHandler<MSG_XTALK>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}