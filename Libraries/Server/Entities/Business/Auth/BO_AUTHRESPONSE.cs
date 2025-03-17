using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Auth;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Auth;

[Mnemonic("autr")]
public class BO_AUTHRESPONSE : IEventHandler<MSG_AUTHRESPONSE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}