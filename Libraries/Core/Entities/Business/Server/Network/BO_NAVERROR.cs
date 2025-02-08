using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Server.Network
{
    [Mnemonic("sErr")]
    public partial class BO_NAVERROR : IEventHandler<MSG_NAVERROR>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}