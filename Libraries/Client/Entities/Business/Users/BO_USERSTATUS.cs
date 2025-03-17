using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("uSta")]
public class BO_USERSTATUS : IEventHandler<MSG_USERSTATUS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}