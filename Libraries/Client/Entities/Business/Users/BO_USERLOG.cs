using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Users;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Users;

[Mnemonic("log ")]
public class BO_USERLOG : IEventHandler<MSG_USERLOG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}