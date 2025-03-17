using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Auth;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Auth;

[Mnemonic("susr")]
public class BO_SUPERUSER : IEventHandler<MSG_SUPERUSER>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}