using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Entities.Network.Shared.Assets;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Common.Server.Entities.Business.Shared.Assets;

[Mnemonic("qAst")]
public partial class BO_ASSETQUERY : IEventHandler<MSG_ASSETQUERY>
{
    public async Task<object?> Handle(object? sender, IEventParams @event)
    {
        throw new NotImplementedException();
    }
}