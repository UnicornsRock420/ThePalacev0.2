using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Client.Network
{
    [Mnemonic("navR")]
    public partial class BO_ROOMGOTO : IEventHandler<MSG_ROOMGOTO>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}