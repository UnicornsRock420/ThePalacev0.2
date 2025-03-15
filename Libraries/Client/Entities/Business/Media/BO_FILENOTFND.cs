using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Server.Media;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Media;

[Mnemonic("fnfe")]
public class BO_FILENOTFND : IEventHandler<MSG_FILENOTFND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}