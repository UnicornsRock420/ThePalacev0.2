using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Shared.Assets
{
    [Mnemonic("qAst")]
    public partial class BO_ASSETQUERY : IIntegrationEventHandler<MSG_ASSETQUERY>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}