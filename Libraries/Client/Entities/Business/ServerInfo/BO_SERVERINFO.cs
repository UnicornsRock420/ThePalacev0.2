using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.ServerInfo;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.ServerInfo;

[Mnemonic("sinf")]
public class BO_SERVERINFO : IEventHandler<MSG_SERVERINFO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}