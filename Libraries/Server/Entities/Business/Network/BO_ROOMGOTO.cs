using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Client.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Network;

[Mnemonic("navR")]
public class BO_ROOMGOTO : IEventHandler<MSG_ROOMGOTO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}