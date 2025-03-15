using ThePalace.Common.Attributes;
using ThePalace.Core.Entities.Network.Shared.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Communications;

[Mnemonic("talk")]
public class BO_TALK : IEventHandler<MSG_TALK>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}