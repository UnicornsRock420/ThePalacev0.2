using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Auth;

[Mnemonic("regi")]
public class BO_LOGON : IEventHandler<MSG_LOGON>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}