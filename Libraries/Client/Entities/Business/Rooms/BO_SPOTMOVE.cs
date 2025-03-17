using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("coLs")]
public class BO_SPOTMOVE : IEventHandler<MSG_SPOTMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}