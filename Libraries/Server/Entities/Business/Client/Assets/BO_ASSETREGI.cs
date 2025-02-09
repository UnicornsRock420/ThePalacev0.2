using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Client.Assets;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Client.Assets
{
    [Mnemonic("rAst")]
    public partial class BO_ASSETREGI : IEventHandler<MSG_ASSETREGI>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}