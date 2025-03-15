using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Rooms;

[Mnemonic("opSn")]
public class BO_SPOTNEW : IEventHandler<MSG_SPOTNEW>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}