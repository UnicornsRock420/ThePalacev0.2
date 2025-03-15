using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Network;

[Mnemonic("tiyr")]
public class BO_TIYID : IEventHandler<MSG_TIYID>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        // TODO: Set Endian on ConnectionState
        
        throw new NotImplementedException();
    }
}