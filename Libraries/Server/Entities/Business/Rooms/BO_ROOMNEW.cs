using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Client.Rooms;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Rooms;

[Mnemonic("nRom")]
public class BO_ROOMNEW : IEventHandler<MSG_ROOMNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}