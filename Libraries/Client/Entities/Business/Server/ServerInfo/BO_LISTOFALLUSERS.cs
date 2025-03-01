using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.ServerInfo;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Client.Entities.Business.Server.ServerInfo;

[Mnemonic("uLst")]
public partial class BO_LISTOFALLUSERS : IEventHandler<MSG_LISTOFALLUSERS>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}