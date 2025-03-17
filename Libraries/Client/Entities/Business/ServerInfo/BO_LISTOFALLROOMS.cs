using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Server.ServerInfo;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.ServerInfo;

[DynamicSize]
[Mnemonic("rLst")]
public class BO_LISTOFALLROOMS : IEventHandler<MSG_LISTOFALLROOMS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}