using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Auth;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Auth;

[Mnemonic("auth")]
public class BO_AUTHENTICATE : IEventHandler<MSG_AUTHENTICATE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}