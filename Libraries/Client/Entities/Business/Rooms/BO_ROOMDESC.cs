using Lib.Common.Attributes;
using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Network.Server.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[DynamicSize]
[Mnemonic("room")]
public class BO_ROOMDESC : IEventHandler<MSG_ROOMDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}