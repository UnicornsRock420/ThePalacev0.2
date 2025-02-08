using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Media;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Media
{
    [Mnemonic("qFil")]
    public partial class BO_FILEQUERY : IIntegrationEventHandler<MSG_FILEQUERY>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}