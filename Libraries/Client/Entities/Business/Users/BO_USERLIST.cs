using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("rprs")]
public class BO_USERLIST : IEventHandler<MSG_USERLIST>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}