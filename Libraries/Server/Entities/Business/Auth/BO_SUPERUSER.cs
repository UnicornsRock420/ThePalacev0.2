using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Auth;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Auth;

[Mnemonic("susr")]
public class BO_SUPERUSER : IEventHandler<MSG_SUPERUSER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}