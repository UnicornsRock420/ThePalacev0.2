using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("opSd")]
public class BO_SPOTDEL : IEventHandler<MSG_SPOTDEL>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}