using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Users;

[Mnemonic("usrC")]
public class BO_USERCOLOR : IEventHandler<MSG_USERCOLOR>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}