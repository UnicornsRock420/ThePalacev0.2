using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("sSta")]
public class BO_SPOTSTATE : IEventHandler<MSG_SPOTSTATE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}