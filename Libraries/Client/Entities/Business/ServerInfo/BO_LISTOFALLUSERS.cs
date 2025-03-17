using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.ServerInfo;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.ServerInfo;

[Mnemonic("uLst")]
public class BO_LISTOFALLUSERS : IEventHandler<MSG_LISTOFALLUSERS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}