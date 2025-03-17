using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.ServerInfo;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.ServerInfo;

[Mnemonic("rLst")]
public class BO_LISTOFALLROOMS : IEventHandler<MSG_LISTOFALLROOMS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}