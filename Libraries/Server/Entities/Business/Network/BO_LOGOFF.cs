using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("bye ")]
public class BO_LOGOFF : IEventHandler<MSG_LOGOFF>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}