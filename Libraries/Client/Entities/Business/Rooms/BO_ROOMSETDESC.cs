using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Client.Entities.Business.Rooms;

[Mnemonic("sRom")]
public class BO_ROOMSETDESC : IEventHandler<MSG_ROOMSETDESC>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}