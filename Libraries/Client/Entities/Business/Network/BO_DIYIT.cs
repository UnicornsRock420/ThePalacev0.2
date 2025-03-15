using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Network;

[Mnemonic("ryit")]
public class BO_DIYIT : IEventHandler<MSG_DIYIT>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        // TODO: Set Endian on ConnectionState
        
        throw new NotImplementedException();
    }
}