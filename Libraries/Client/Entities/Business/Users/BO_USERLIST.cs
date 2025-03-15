using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Users;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Users;

[Mnemonic("rprs")]
public class BO_USERLIST : IEventHandler<MSG_USERLIST>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}