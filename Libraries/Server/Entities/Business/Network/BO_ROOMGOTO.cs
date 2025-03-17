using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("navR")]
public class BO_ROOMGOTO : IEventHandler<MSG_ROOMGOTO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}