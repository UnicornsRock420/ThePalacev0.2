using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Client.Media;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Media;

[Mnemonic("qFil")]
public class BO_FILEQUERY : IEventHandler<MSG_FILEQUERY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}