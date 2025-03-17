using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("nprs")]
public class BO_USERNEW : IEventHandler<MSG_USERNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}