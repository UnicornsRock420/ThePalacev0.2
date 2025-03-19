using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Auth;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Auth;

[Mnemonic("rep2")]
public class BO_ALTLOGONREPLY : IEventHandler<MSG_ALTLOGONREPLY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        // TODO

        return null;
    }
}