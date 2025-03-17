using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("endr")]
public class BO_ROOMDESCEND : IEventHandler<MSG_ROOMDESCEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}