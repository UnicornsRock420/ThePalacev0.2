using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Network;

[Mnemonic("rep2")]
public class BO_ALTLOGONREPLY : IEventHandler<MSG_ALTLOGONREPLY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}