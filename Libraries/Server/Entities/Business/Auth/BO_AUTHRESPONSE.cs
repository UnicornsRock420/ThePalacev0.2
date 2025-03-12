using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Auth;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Auth;

[Mnemonic("autr")]
public class BO_AUTHRESPONSE : IEventHandler<MSG_AUTHRESPONSE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}