using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("usrP")]
public class BO_USERPROP : IEventHandler<MSG_USERPROP>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}