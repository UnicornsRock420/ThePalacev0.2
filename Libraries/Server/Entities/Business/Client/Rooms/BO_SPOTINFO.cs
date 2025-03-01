using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Client.Rooms;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Client.Rooms;

[Mnemonic("ofNs")]
public partial class BO_SPOTINFO : IEventHandler<MSG_SPOTINFO>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}