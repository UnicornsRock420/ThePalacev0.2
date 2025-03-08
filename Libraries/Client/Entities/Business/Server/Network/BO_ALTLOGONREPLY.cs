using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Server.Network;

[Mnemonic("rep2")]
public class BO_ALTLOGONREPLY : IEventHandler<MSG_ALTLOGONREPLY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}