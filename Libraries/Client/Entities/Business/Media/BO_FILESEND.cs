using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Server.Media;

[Mnemonic("sFil")]
public class BO_FILESEND : IEventHandler<MSG_FILESEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}