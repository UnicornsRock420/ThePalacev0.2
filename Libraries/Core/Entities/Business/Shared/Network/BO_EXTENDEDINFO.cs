using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Business.Shared.Network
{
    [Mnemonic("sInf")]
    public partial class BO_EXTENDEDINFO : IEventHandler<MSG_EXTENDEDINFO>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}