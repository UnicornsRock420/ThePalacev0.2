using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Shared.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("pLoc")]
public class BO_PICTMOVE : IEventHandler<MSG_PICTMOVE>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}