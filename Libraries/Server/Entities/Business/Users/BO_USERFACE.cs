using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Users;

[Mnemonic("usrF")]
public class BO_USERFACE : IEventHandler<MSG_USERFACE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}