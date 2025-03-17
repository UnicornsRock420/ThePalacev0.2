using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("unlo")]
public class BO_DOORUNLOCK : IEventHandler<MSG_DOORUNLOCK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}