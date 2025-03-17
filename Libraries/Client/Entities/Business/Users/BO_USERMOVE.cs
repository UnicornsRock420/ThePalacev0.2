using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("uLoc")]
public class BO_USERMOVE : IEventHandler<MSG_USERMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}