using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Assets;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Server.Assets
{
    [Mnemonic("sAst")]
    public partial class BO_ASSETSEND : IIntegrationEventHandler<MSG_ASSETSEND>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}