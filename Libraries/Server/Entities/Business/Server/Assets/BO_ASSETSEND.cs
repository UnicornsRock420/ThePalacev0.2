using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Server.Assets;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Server.Assets;

[Mnemonic("sAst")]
public partial class BO_ASSETSEND : IEventHandler<MSG_ASSETSEND>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}