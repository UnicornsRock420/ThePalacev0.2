using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Communications;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Client.Communications;

[Mnemonic("rmsg")]
public class BO_RMSG : IEventHandler<MSG_RMSG>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}