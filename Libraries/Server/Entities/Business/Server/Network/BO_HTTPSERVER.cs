using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Network.Server.Network;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Common.Server.Entities.Business.Server.Network
{
    [DynamicSize]
    [Mnemonic("HTTP")]
    public partial class BO_HTTPSERVER : IEventHandler<MSG_HTTPSERVER>
    {
        public async Task<object?> Handle(object? sender, IEventParams @event)
        {
            throw new NotImplementedException();
        }
    }
}