using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("opSn")]
public class BO_SPOTNEW : IEventHandler<MSG_SPOTNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}