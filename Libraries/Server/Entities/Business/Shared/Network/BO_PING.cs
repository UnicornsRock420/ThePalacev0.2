using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Shared.Network;

[Mnemonic("ping")]
public partial class BO_PING : IEventHandler<MSG_PING>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}