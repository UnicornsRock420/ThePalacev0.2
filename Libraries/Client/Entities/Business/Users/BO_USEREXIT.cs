using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("eprs")]
public class BO_USEREXIT : IEventHandler<MSG_USEREXIT>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}