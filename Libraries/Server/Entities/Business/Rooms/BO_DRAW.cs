using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("draw")]
public class BO_DRAW : IEventHandler<MSG_DRAW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}