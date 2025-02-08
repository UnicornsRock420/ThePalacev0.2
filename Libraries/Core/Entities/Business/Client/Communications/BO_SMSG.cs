using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces.Core;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Business.Client.Communications
{
    [Mnemonic("smsg")]
    public partial class BO_SMSG : IIntegrationEventHandler<MSG_SMSG>
    {
        public async Task<object?> Handle(object? sender, IIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}