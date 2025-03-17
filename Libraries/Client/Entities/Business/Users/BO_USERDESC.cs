using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("usrD")]
public class BO_USERDESC : IEventHandler<MSG_USERDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}