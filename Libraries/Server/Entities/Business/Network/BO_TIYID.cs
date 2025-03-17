using Lib.Common.Attributes;
using Lib.Core.Entities.Network.Server.Network;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Common.Server.Entities.Business.Network;

[Mnemonic("tiyr")]
public class BO_TIYID : IEventHandler<MSG_TIYID>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}